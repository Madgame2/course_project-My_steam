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
    /// Логика взаимодействия для PlayInfo.xaml
    /// </summary>
    public partial class PlayInfo : UserControl
    {
     
        public TimeSpan PlayTime {  get; set; }
        public DateTime lastLaynch { get; set; }
        public PlayInfo()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
