﻿namespace TemplateMAUI.Controls
{
    public class ComparerThumb : TemplatedView
    {
        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create(nameof(Orientation), typeof(ComparerOrientation), typeof(ComparerThumb), ComparerOrientation.Horizontal,
                propertyChanged: OnOrientationChanged);

        static void OnOrientationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ComparerThumb)?.UpdateOrientation();
        }

        public ComparerOrientation Orientation
        {
            get => (ComparerOrientation)GetValue(OrientationProperty);
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(nameof(Color), typeof(Color), typeof(ComparerThumb), Colors.Transparent,
                propertyChanged: OnColorChanged);

        static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ComparerThumb)?.UpdateColors();
        }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set { SetValue(ColorProperty, value); }
        }

        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(ComparerThumb), Colors.Transparent,
                propertyChanged: OnBackgroundColorChanged);

        static void OnBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ComparerThumb)?.UpdateColors();
        }

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set { SetValue(BackgroundColorProperty, value); }
        }

        void UpdateOrientation()
        {
            if (Orientation == ComparerOrientation.Horizontal)
            {
                Application.Current.Resources.TryGetValue("HorizontalComparerThumb", out object horizontalComparerThumb);
                ControlTemplate = horizontalComparerThumb as ControlTemplate;
            }
            else
            {
                Application.Current.Resources.TryGetValue("VerticalComparerThumb", out object verticalComparerThumb);
                ControlTemplate = verticalComparerThumb as ControlTemplate;
            }
        }

        void UpdateColors()
        {
            if (BackgroundColor == Colors.Transparent)
                BackgroundColor = LighterColor(Color);
        }

        Color LighterColor(Color color, float correctionFactory = 75f)
        {
            correctionFactory /= 100f;

            const float rgb255 = 255f;

            return Color.FromRgb(
                (int)((float)color.Red + ((rgb255 - (float)color.Red) * correctionFactory)),
                (int)((float)color.Green + ((rgb255 - (float)color.Green) * correctionFactory)),
                (int)((float)color.Blue + ((rgb255 - (float)color.Blue) * correctionFactory)));
        }
    }
}