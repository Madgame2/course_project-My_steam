using My_steam_client.Scripts.Interfaces;
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
    /// Логика взаимодействия для SideFilters.xaml
    /// </summary>
    public partial class SideFilters : UserControl
    {

        
        public SideFilters()
        {
            InitializeComponent();
            DataContext = new SideFiltersViewModel();
        }

        public string getQueryFilters()
        {
            var viewModel = DataContext as SideFiltersViewModel;
            if (viewModel != null)
            {
                var allParams = viewModel.Filters.SelectMany(f => f.ToQueryParameters()).Where(kv => !string.IsNullOrEmpty(kv.Value));

                return string.Join("&", allParams.Select(kv =>
                    $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));
            }

            return string.Empty;
        }
    }
}
