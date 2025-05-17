using My_steam_client.Templates;
using My_steam_client.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Globalization;
using System.Text.RegularExpressions;
using Game_Net_DTOLib;

namespace My_steam_client
{
    /// <summary>
    /// Логика взаимодействия для CreateRedactProjectWindow.xaml
    /// </summary>
    public partial class CreateRedactProjectWindow : Window
    {
        public CreateRedactProjectWindow(string header_Titel)
        {
            InitializeComponent();
            HeaderContaner.Content = new Header(this, header_Titel);
            var vm = new EditProjectDataViewModel();
            vm.ShowLoadingWindowRequested += OnShowLoadingWindowRequested;
            vm.CloseWindow += () => this.Close();
            DataContext = vm;
        }

        private void OnShowLoadingWindowRequested(ProjectUploadDto dto)
        {
            var loadingWindow = new UploadingWindow(dto);
            loadingWindow.Show();
        }

        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            var currentText = textBox.Text;
            var selectionStart = textBox.SelectionStart;
            var selectionLength = textBox.SelectionLength;

            // Разрешаем оба разделителя
            string newText = currentText.Remove(selectionStart, selectionLength)
                .Insert(selectionStart, e.Text);

            // Регулярка: цифры, один разделитель (точка или запятая), до двух знаков после разделителя
            string pattern = @"^\d{0,}([.,]{1}\d{0,2})?$";

            e.Handled = !Regex.IsMatch(newText, pattern);
        }

        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string text = textBox.Text.Replace(',', '.'); // заменяем запятую на точку

            if (string.IsNullOrEmpty(text))
            {
                textBox.Background = new SolidColorBrush(Color.FromRgb(29, 29, 29)); // #FF1D1D1D
                return;
            }

            if (decimal.TryParse(text, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal result))
            {
                textBox.Background = new SolidColorBrush(Color.FromRgb(29, 29, 29)); // #FF1D1D1D
            }
            else
            {
                textBox.Background = new SolidColorBrush(Color.FromRgb(255, 99, 71)); // LightCoral
            }
        }
    }
}
