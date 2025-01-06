using System.Reflection;
using System.Runtime.CompilerServices;

namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The GridSplitter is a custom templated control designed to provide a way to resize rows or columns in a grid layout. 
    /// It allows users to dynamically adjust the size of grid elements, enhancing the flexibility and interactivity of the user interface.
    /// </summary>
    public class GridSplitter : TemplatedView
    {
        const string ElementGridSplitter = "PART_GridSplitter";
        const string ElementIndicator = "PART_GridSplitterIndicator";

        Grid _gridSplitter;
        Layout _indicator;

        double _previousPositionX;
        double _previousPositionY;

        public static readonly BindableProperty ElementProperty =
            BindableProperty.Create(nameof(Element), typeof(View), typeof(GridSplitter), null);

        public View Element
        {
            get => (View)GetValue(ElementProperty);
            set => SetValue(ElementProperty, value);
        }

        public static readonly BindableProperty ResizeDirectionProperty =
            BindableProperty.Create(nameof(ResizeDirection), typeof(GridResizeDirection), typeof(GridSplitter), GridResizeDirection.Auto);

        public GridResizeDirection ResizeDirection
        {
            get => (GridResizeDirection)GetValue(ResizeDirectionProperty);
            set => SetValue(ResizeDirectionProperty, value);
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(GridSplitter), Colors.LightGray);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _gridSplitter = GetTemplateChild(ElementGridSplitter) as Grid;
            _indicator = GetTemplateChild(ElementIndicator) as Layout;

            UpdateIndicator();
            UpdateIsEnabled();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ResizeDirectionProperty.PropertyName)
            {
                UpdateIndicator();
                UpdateLayout();
            }
            else if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        void UpdateIsEnabled()
        {
            if (_gridSplitter == null)
                return;

            if (IsEnabled)
            {
                var panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += OnPanUpdated;
                _gridSplitter.GestureRecognizers.Add(panGestureRecognizer);
            }
            else
            {
                // TODO: Remove only specific gestures.
                _gridSplitter.GestureRecognizers.Clear();
            }
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        var totalX = e.TotalX - _previousPositionX;
                        var totalY = e.TotalY - _previousPositionY;

                        UpdateLayout(totalX, totalY);

                        _previousPositionX = e.TotalX;
                        _previousPositionY = e.TotalY;
                    }
                    else
                        UpdateLayout(e.TotalX, e.TotalY);
                    break;
                case GestureStatus.Started:
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    break;
            }
        }

        void UpdateIndicator()
        {
            if (_indicator is null)
                return;

            if (ResizeDirection == GridResizeDirection.Columns)
                _indicator.Rotation = 90;
            else
                _indicator.Rotation = 0;
        }

        void UpdateLayout(double offsetX = 0, double offsetY = 0)
        {
            if (Parent is not Grid)
                // TODO: Throw Exception?
                return;

            if (ResizeDirection == GridResizeDirection.Columns)
                UpdateColumns(offsetX);
            else
                UpdateRows(offsetY);
        }

        void UpdateColumns(double offsetX)
        {
            if (offsetX == 0)
                return;

            var grid = Parent as Grid;
            int column = Grid.GetColumn(this);

            int columnCount = grid.ColumnDefinitions.Count();

            if (columnCount <= 1 || column == 0 || column == columnCount - 1)
                return;

            ColumnDefinition previousColumn = grid.ColumnDefinitions[column - 1];

            double previousRowWidth;

            if (previousColumn.Width.IsAbsolute)
                previousRowWidth = previousColumn.Width.Value;
            else
                previousRowWidth = (double)previousColumn.GetType().GetRuntimeProperties().First((p) => p.Name == "ActualWidth").GetValue(previousColumn);

            double actualWidth = previousRowWidth + offsetX;

            if (actualWidth < 0)
                actualWidth = 0;

            previousColumn.Width = new GridLength(actualWidth);
        }

        void UpdateRows(double offsetY)
        {
            if (offsetY == 0)
                return;

            var grid = Parent as Grid;
            var row = Grid.GetRow(this);
            int rowCount = grid.RowDefinitions.Count();

            if (rowCount <= 1 || row == 0 || row == rowCount - 1)
                return;

            RowDefinition previousRow = grid.RowDefinitions[row - 1];

            double previousRowHeight;

            if (previousRow.Height.IsAbsolute)
                previousRowHeight = previousRow.Height.Value;
            else
                previousRowHeight = (double)previousRow.GetType().GetRuntimeProperties().First((p) => p.Name == "ActualHeight").GetValue(previousRow);

            var actualHeight = previousRowHeight + offsetY;

            if (actualHeight < 0)
                actualHeight = 0;

            previousRow.Height = new GridLength(actualHeight);
        }
    }
}