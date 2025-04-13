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
    /// Логика взаимодействия для Showcase.xaml
    /// </summary>
    public partial class Showcase : UserControl
    {

        public ObservableCollection<ShowCaseObject> _items { get; } = new();

        public Showcase()
        {
            InitializeComponent();
        }

        public void addObject(ShowCaseObject obj) { 
          _items.Add(obj);
        }


        public void addObject(IEnumerable<ShowCaseObject> objects) {
            foreach (var obj in objects) { 
                _items.Add((ShowCaseObject)obj);
            }
        }
    }
}
