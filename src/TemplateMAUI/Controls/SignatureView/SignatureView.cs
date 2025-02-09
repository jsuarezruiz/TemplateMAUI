using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace TemplateMAUI.Controls
{
    public class SignatureView : TemplatedView
    {
        const string ElementContainer = "PART_Container";
        const string ElementGraphics = "PART_Graphics";

        Border _container;
        GraphicsView _graphicsView;

        public static readonly BindableProperty StrokeColorProperty =
           BindableProperty.Create(nameof(StrokeColor), typeof(Color), typeof(SignatureView), Colors.Black,
               propertyChanged: OnStrokeColorChanged);

        static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SignatureView).UpdateStrokeColor();
        }

        public Color StrokeColor
        {
            get { return (Color)GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }

        public static readonly BindableProperty StrokeThicknessProperty =
           BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(SignatureView), 4.0d,
               propertyChanged: OnStrokeThicknessChanged);

        static void OnStrokeThicknessChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SignatureView).UpdateStrokeThickness();
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static new readonly BindableProperty BackgroundProperty =
            BindableProperty.Create(nameof(Background), typeof(Brush), typeof(SignatureView));

        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly BindableProperty BorderBrushProperty =
            BindableProperty.Create(nameof(BorderBrush), typeof(Brush), typeof(SignatureView));

        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly BindableProperty BorderThicknessProperty =
            BindableProperty.Create(nameof(BorderThickness), typeof(double), typeof(SignatureView), 1.0d);

        public double BorderThickness
        {
            get { return (double)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(SignatureView), new CornerRadius(0),
                propertyChanged: OnCornerRadiusChanged);

        static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SignatureView).UpdateCornerRadius();
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty ClearCommandProperty = 
            BindableProperty.Create(nameof(ClearCommand), typeof(ICommand), typeof(SignatureView));

        public ICommand ClearCommand
        {
            get => (ICommand)GetValue(ClearCommandProperty);
            set { SetValue(ClearCommandProperty, value); }
        }

        public event EventHandler StrokeStarted;
        public event EventHandler StrokeCompleted;
        public event EventHandler Cleared;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = GetTemplateChild(ElementContainer) as Border;
            _graphicsView = GetTemplateChild(ElementGraphics) as GraphicsView;

            if (_graphicsView is not null)
                _graphicsView.Drawable = new SignatureDrawable();

            UpdateIsEnabled();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        public async Task SaveAsync(string fileName)
        {
            if (_graphicsView is null)
                return;

            IScreenshotResult screenshotResult = await _graphicsView.CaptureAsync();

            if (screenshotResult is not null)
            {
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var targetFile = System.IO.Path.Combine(filePath, $"{fileName}.png");

                using FileStream outputStream = File.Create(targetFile);
                if (File.Exists(targetFile))
                {
                    await screenshotResult.CopyToAsync(outputStream, ScreenshotFormat.png);
                }
            }
        }

        public void Clear()
        {
            if (_graphicsView?.Drawable is SignatureDrawable signatureDrawable)
            {
                signatureDrawable.SignaturePoints.Clear();
                _graphicsView.Invalidate();
            }

            if (ClearCommand is not null)
                ClearCommand.Execute(null);

            Cleared?.Invoke(this, EventArgs.Empty);
        }

        void UpdateIsEnabled()
        {
            if (_graphicsView is null)
                return;

            if (IsEnabled)
            {
                _graphicsView.StartInteraction += OnGraphicsViewStartInteraction;
                _graphicsView.DragInteraction += OnGraphicsViewDragInteraction;
                _graphicsView.EndInteraction += OnGraphicsViewEndInteraction;
            }
            else
            {
                _graphicsView.StartInteraction -= OnGraphicsViewStartInteraction;
                _graphicsView.DragInteraction -= OnGraphicsViewDragInteraction;
                _graphicsView.EndInteraction -= OnGraphicsViewEndInteraction;
            }
        }

        void UpdateStrokeColor()
        {
            if (_graphicsView?.Drawable is SignatureDrawable signatureDrawable)
            {
                signatureDrawable.StrokeColor = StrokeColor;
                _graphicsView.Invalidate();
            }
        }

        void UpdateStrokeThickness()
        {
            if (_graphicsView?.Drawable is SignatureDrawable signatureDrawable)
            {
                signatureDrawable.StrokeThickness = StrokeThickness;
                _graphicsView.Invalidate();
            }
        }

        void UpdateCornerRadius()
        {
            if (_container is not null && _container.StrokeShape is RoundRectangle strokeShape)
            {
                strokeShape.CornerRadius = CornerRadius;
            }
        }

        void OnGraphicsViewStartInteraction(object sender, TouchEventArgs e)
        {
            if (_graphicsView?.Drawable is SignatureDrawable signatureDrawable)
            {
                signatureDrawable.SignaturePoints.Clear();
                signatureDrawable.SignatureStartPoint = e.Touches[0];
                _graphicsView.Invalidate();

                StrokeStarted?.Invoke(this, EventArgs.Empty);
            }
        }

        void OnGraphicsViewDragInteraction(object sender, TouchEventArgs e)
        {
            if (_graphicsView?.Drawable is SignatureDrawable signatureDrawable)
            {
                signatureDrawable.SignaturePoints.Add(e.Touches[0]);
                _graphicsView.Invalidate();
            }
        }

        void OnGraphicsViewEndInteraction(object sender, TouchEventArgs e)
        {
            StrokeCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}