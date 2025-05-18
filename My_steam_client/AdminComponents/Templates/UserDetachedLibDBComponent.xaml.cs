using My_steam_client.AdminComponents.ModelsView;
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

namespace My_steam_client.AdminComponents.Templates
{
    /// <summary>
    /// Логика взаимодействия для UserDetachedLibDBComponent.xaml
    /// </summary>
    public partial class UserDetachedLibDBComponent : UserControl
    {
        public UserDetachedLibDBComponent()
        {
            InitializeComponent();
            DataContext = new DetachedLibModelView();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "IsNew")
            {
                e.Cancel = true;
            }
        }
    }
}
