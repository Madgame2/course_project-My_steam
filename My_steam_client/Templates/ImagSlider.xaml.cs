using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace My_steam_client.Templates
{
    /// <summary>
    /// Логика взаимодействия для ImagSlider.xaml
    /// </summary>
    public partial class ImagSlider : UserControl
    {
        public ObservableCollection<SliderImage> _sliderImages { get; set; }

        public SliderImage selectedImage { get; set; }

        private int currentIndex = 0;

        public ImagSlider()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            currentIndex--;

            if (currentIndex < 0)
            {
                currentIndex = _sliderImages.Count-1;
            }

            UpadateSlider();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentIndex++;

            if (currentIndex >= _sliderImages.Count)
            {
                currentIndex = 0;
            }
            UpadateSlider();
        }

        private void UpadateSlider()
        {
            selectedImage.isActive = false;

            selectedImage = _sliderImages[currentIndex];
            selectedImage.isActive=true;

            Scroll?.ScrollToHorizontalOffset(currentIndex * 50);
        }

        private void onImageClick(object sender, MouseButtonEventArgs e)
        {
            var clickedElement = (FrameworkElement)sender;

            var item = clickedElement.DataContext as SliderImage;
            if (item != null)
            {

                var index = _sliderImages.IndexOf(item);


                currentIndex = index;

                selectedImage.isActive = false;

                selectedImage = _sliderImages[currentIndex];
                selectedImage.isActive = true;

            }
        }
    }
}
