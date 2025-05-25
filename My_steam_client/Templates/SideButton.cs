using System;
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
    public class SideButton : ToggleButton
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


        // Заголовок текста
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(SideButton), new PropertyMetadata("Моё приложение"));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // Размер шрифта
        public static readonly DependencyProperty TitleFontSizeProperty =
            DependencyProperty.Register(nameof(TitleFontSize), typeof(double), typeof(SideButton), new PropertyMetadata(14.0));

        public double TitleFontSize
        {
            get => (double)GetValue(TitleFontSizeProperty);
            set => SetValue(TitleFontSizeProperty, value);
        }

        // Начертание (жирный, обычный и т.п.)
        public static readonly DependencyProperty TitleFontWeightProperty =
            DependencyProperty.Register(nameof(TitleFontWeight), typeof(FontWeight), typeof(SideButton), new PropertyMetadata(FontWeights.Normal));

        public FontWeight TitleFontWeight
        {
            get => (FontWeight)GetValue(TitleFontWeightProperty);
            set => SetValue(TitleFontWeightProperty, value);
        }

        // Стиль шрифта (обычный, курсив и т.д.)
        public static readonly DependencyProperty TitleFontStyleProperty =
            DependencyProperty.Register(nameof(TitleFontStyle), typeof(FontStyle), typeof(SideButton), new PropertyMetadata(FontStyles.Normal));

        public FontStyle TitleFontStyle
        {
            get => (FontStyle)GetValue(TitleFontStyleProperty);
            set => SetValue(TitleFontStyleProperty, value);
        }

        // Шрифт (название семейства, например Segoe UI, Consolas и т.д.)
        public static readonly DependencyProperty TitleFontFamilyProperty =
            DependencyProperty.Register(nameof(TitleFontFamily), typeof(FontFamily), typeof(SideButton), new PropertyMetadata(new FontFamily("Segoe UI")));

        public FontFamily TitleFontFamily
        {
            get => (FontFamily)GetValue(TitleFontFamilyProperty);
            set => SetValue(TitleFontFamilyProperty, value);
        }

        // Межстрочное расстояние
        public static readonly DependencyProperty TitleLineHeightProperty =
            DependencyProperty.Register(nameof(TitleLineHeight), typeof(double), typeof(SideButton), new PropertyMetadata(double.NaN));

        public double TitleLineHeight
        {
            get => (double)GetValue(TitleLineHeightProperty);
            set => SetValue(TitleLineHeightProperty, value);
        }

        // Растяжение шрифта (узкий, обычный, растянутый и т.д.)
        public static readonly DependencyProperty TitleFontStretchProperty =
            DependencyProperty.Register(nameof(TitleFontStretch), typeof(FontStretch), typeof(SideButton), new PropertyMetadata(FontStretches.Normal));

        public FontStretch TitleFontStretch
        {
            get => (FontStretch)GetValue(TitleFontStretchProperty);
            set => SetValue(TitleFontStretchProperty, value);
        }


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
