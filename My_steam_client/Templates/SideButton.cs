﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace My_steam_client.Templates
{
    class SideButton : ToggleButton
    {


        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(string), typeof(SideButton),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(SideButton), new PropertyMetadata(50.0));

        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(SideButton), new PropertyMetadata(50.0));

        public static readonly DependencyProperty IconTranslateXProperty =
            DependencyProperty.Register(nameof(IconTranslateX), typeof(double), typeof(SideButton), new PropertyMetadata(0.0));

        public static readonly DependencyProperty IconTranslateYProperty =
            DependencyProperty.Register(nameof(IconTranslateY), typeof(double), typeof(SideButton), new PropertyMetadata(0.0));

        public static readonly DependencyProperty SvgStrokeColorProperty =
            DependencyProperty.Register(nameof(SvgStrokeColor),typeof(Brush),typeof(SideButton),new PropertyMetadata(null));

        public static readonly DependencyProperty HoverTextBrushProperty =
            DependencyProperty.Register(nameof(HoverTextBrush), typeof(Brush), typeof(SideButton), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty HoverImageCollorProperty =
            DependencyProperty.Register(nameof(HoverImageCollor), typeof(Brush), typeof(SideButton), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty ActiveTextColorProperty =
            DependencyProperty.Register(nameof(ActiveTextColor),typeof(Brush),typeof(SideButton),new PropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty ActiveImageColorProperty =
            DependencyProperty.Register(nameof(ActiveImageColor),typeof(Brush),typeof(SideButton),new PropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty ActiveBacgroundProperty =
            DependencyProperty.Register(nameof(ActiveBacground),typeof(Brush),typeof(SideButton),new PropertyMetadata(Brushes.Blue));


        public Brush ActiveBacground
        {
            get=>(Brush)GetValue(ActiveBacgroundProperty);
            set=>SetValue(ActiveBacgroundProperty, value);
        }
        public Brush ActiveImageColor
        {
            get=>(Brush)GetValue(ActiveImageColorProperty);
            set=> SetValue(ActiveImageColorProperty, value);
        }
        public Brush ActiveTextColor
        {
            get => (Brush)GetValue(ActiveTextColorProperty);
            set => SetValue(ActiveTextColorProperty, value);
        }
        public Brush HoverImageCollor
        {
            get =>(Brush)GetValue(HoverImageCollorProperty);
            set =>SetValue(HoverImageCollorProperty, value);
        }
        public Brush HoverTextBrush
        {
            get => (Brush)GetValue(HoverTextBrushProperty);
            set => SetValue(HoverTextBrushProperty, value);
        }
        public Brush SvgStrokeColor
        {
            get => (Brush)GetValue(SvgStrokeColorProperty);
            set =>SetValue(SvgStrokeColorProperty, value);
        }

        public double IconTranslateX
        {
            get => (double)GetValue(IconTranslateXProperty);
            set => SetValue(IconTranslateXProperty, value);
        }

        public double IconTranslateY
        {
            get => (double)GetValue(IconTranslateYProperty);
            set => SetValue(IconTranslateYProperty, value);
        }

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public double IconWidth
        {
            get => (double)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        public double IconHeight
        {
            get => (double)GetValue(IconHeightProperty);
            set => SetValue(IconHeightProperty, value);
        }

        public static readonly DependencyProperty IconImageProperty =
                DependencyProperty.Register(nameof(IconImage), typeof(BitmapImage), typeof(SideButton), new PropertyMetadata(null));

        public BitmapImage IconImage
        {
            get => (BitmapImage)GetValue(IconImageProperty);
            private set => SetValue(IconImageProperty, value);
        }

        protected override void OnClick()
        {
            if (IsChecked == true)
                return;

            
            base.OnClick();
        }


        private void UpdateIconImage()
        {
            if (string.IsNullOrEmpty(Icon))
            {
                IconImage = null;
                return;
            }

            try
            {
                var uri = new Uri(Icon, UriKind.RelativeOrAbsolute);
                IconImage = new BitmapImage(uri);
            }
            catch (Exception)
            {
                IconImage = null;
            }
        }


        static SideButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SideButton),
                new FrameworkPropertyMetadata(typeof(SideButton)));
        }
    }
}
