﻿using Microsoft.Maui.Controls.Shapes;

namespace TemplateMAUI.Controls
{
    /// <summary>
    /// The RateItem is a custom templated control designed to represent a single item within a rating control. 
    /// It provides the visual representation and functionality for individual rating elements, such as stars or other icons, making up the complete rating interface.
    /// </summary>
    public class RateItem : TemplatedView
    {
        const string ElementIcon = "PART_Icon";

        View _icon;

        public static readonly BindableProperty ItemSizeProperty =
            BindableProperty.Create(nameof(ItemSize), typeof(double), typeof(RateItem), 30.0d);

        public double ItemSize
        {
            get => (double)GetValue(ItemSizeProperty);
            set => SetValue(ItemSizeProperty, value);
        }

        public static readonly BindableProperty SelectedFillProperty =
            BindableProperty.Create(nameof(SelectedFill), typeof(Color), typeof(RateItem), Color.FromArgb("#F6C602"));

        public Color SelectedFill
        {
            get => (Color)GetValue(SelectedFillProperty);
            set => SetValue(SelectedFillProperty, value);
        }

        public static readonly BindableProperty UnSelectedFillProperty =
            BindableProperty.Create(nameof(UnSelectedFill), typeof(Color), typeof(RateItem), Colors.Transparent);

        public Color UnSelectedFill
        {
            get => (Color)GetValue(UnSelectedFillProperty);
            set => SetValue(UnSelectedFillProperty, value);
        }

        public static readonly BindableProperty SelectedStrokeProperty =
            BindableProperty.Create(nameof(SelectedStroke), typeof(Color), typeof(RateItem), Color.FromArgb("#F6C602"));

        public Color SelectedStroke
        {
            get => (Color)GetValue(SelectedStrokeProperty);
            set => SetValue(SelectedStrokeProperty, value);
        }

        public static readonly BindableProperty UnSelectedStrokeProperty =
            BindableProperty.Create(nameof(UnSelectedStroke), typeof(Color), typeof(RateItem), Application.Current.RequestedTheme == AppTheme.Light ? Colors.Black : Colors.White);

        public Color UnSelectedStroke
        {
            get => (Color)GetValue(UnSelectedStrokeProperty);
            set => SetValue(UnSelectedStrokeProperty, value);
        }

        public static readonly BindableProperty SelectedStrokeWidthProperty =
            BindableProperty.Create(nameof(SelectedStrokeWidth), typeof(double), typeof(RateItem), 1.0d);

        public double SelectedStrokeWidth
        {
            get => (double)GetValue(SelectedStrokeWidthProperty);
            set => SetValue(SelectedStrokeWidthProperty, value);
        }

        public static readonly BindableProperty UnSelectedStrokeWidthProperty =
            BindableProperty.Create(nameof(UnSelectedStrokeWidth), typeof(double), typeof(RateItem), 1.0d);

        public double UnSelectedStrokeWidth
        {
            get => (double)GetValue(UnSelectedStrokeWidthProperty);
            set => SetValue(UnSelectedStrokeWidthProperty, value);
        }

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(nameof(Icon), typeof(Geometry), typeof(RateItem), null);

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly BindableProperty IsReadOnlyProperty =
            BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(RateItem), false);

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(RateItem), false);

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _icon = GetTemplateChild(ElementIcon) as View;
            _icon.WidthRequest = Width;
        }
    }
}