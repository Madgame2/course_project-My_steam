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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace My_steam_client.Templates
{
    /// <summary>
    /// Логика взаимодействия для SendingErrorComponent.xaml
    /// </summary>
    public partial class SendingErrorComponent : UserControl
    {

        public Action CloseButtonnPrsed;
        public Action TryAgainPrsed;

        public SendingErrorComponent()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CloseButtonnPrsed?.Invoke();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TryAgainPrsed?.Invoke();
        }
    }
}
