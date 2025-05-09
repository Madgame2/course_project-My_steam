using My_steam_client.Templates;
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

namespace My_steam_client
{
    /// <summary>
    /// Логика взаимодействия для YesNoDialog.xaml
    /// </summary>
    public partial class YesNoDialog : Window 
    {
        public string Header {  get; private set; }
        public string Description { get; private set; }

        public bool Result { get; set; } = false;
        public YesNoDialog(string Tittle, string Header, string description)
        {
            InitializeComponent();

            this.Title = Tittle;
            this.Header = Header;
            this.Description = description;

            DataContext = this;
            HeaderContaner.Content = new Header(this, Tittle);
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            this.DialogResult = true;
        }
        private void No_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            this.DialogResult = false;
        }
    }
}
