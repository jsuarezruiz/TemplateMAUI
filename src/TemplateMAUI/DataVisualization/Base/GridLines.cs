using Microsoft.Maui.Controls.Shapes;

namespace TemplateMAUI.DataVisualization
{
    public class GridLines : TemplatedView
    {
        const string ElementGridLayout = "PART_GridLayout";

        AbsoluteLayout _gridLayout;
        List<double> _locations;
        readonly List<BoxView> _gridLines;

        public GridLines()
        {
            _locations = new List<double>();
            _gridLines = new List<BoxView>();

            VerticalOptions = LayoutOptions.Fill;
            HorizontalOptions = LayoutOptions.Fill;

            // TODO: Unsubscribe 
            LayoutChanged += OnGridLinesLayoutUpdated;
        }

        void OnGridLinesLayoutUpdated(object sender, object e)
        {
            UpdateGridLines();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _gridLayout = GetTemplateChild(ElementGridLayout) as AbsoluteLayout;
   
        }

        public void UpdateLocations(IEnumerable<double> locations)
        {
            _locations = new List<double>(locations);
            UpdateGridLines();
        }

        void UpdateGridLines()
        {
            int count = Math.Min(_locations.Count, _gridLines.Count);

            UpdateLineLocations(count);

            if (_locations.Count != _gridLines.Count)
            {
                if (_locations.Count > _gridLines.Count)
                    AddGridLines(count);
                else if (_locations.Count < _gridLines.Count)
                    RemoveGridLines(count);
            }
        }

        void UpdateLineLocations(int count)
        {
            for (int i = 0; i < count; i++)
            {
                UpdateLineLocation(i);
            }
        }

        void UpdateLineLocation(int i)
        {
            if (_gridLines.Count > i && _gridLines[i].TranslationY != _locations[i])
            {
                var max = _locations.Max();
                var height = _gridLayout.Frame.Height;
                var y = (height - (_locations[i] * height / max)) + 1;

                _gridLines[i].TranslationY = y;
                _gridLines[i].WidthRequest = _gridLayout.Width;
            }
        }

        void AddGridLines(int count)
        {
            for (int i = count; i < _locations.Count; i++)
            {
                BoxView line = new BoxView
                {
                    Background = new SolidColorBrush(Colors.LightGray),
                    HeightRequest = 1,
                    WidthRequest = _gridLayout.Width,
                };
                _gridLines.Add(line);

                UpdateLineLocation(i);

                _gridLayout.Children.Add(_gridLines[i]);
            }
        }

        void RemoveGridLines(int count)
        {
            for (int i = _gridLines.Count - 1; i >= count; i--)
            {
                _gridLayout.Children.Remove(_gridLines[i]);
                _gridLines.RemoveAt(i);
            }
        }
    }
}