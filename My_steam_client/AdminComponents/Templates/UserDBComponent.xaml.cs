using System.Collections.Specialized;
using System.Windows.Controls;
using My_steam_client.AdminComponents.ModelsView;
using My_steam_client.AdminComponents.Models;

namespace My_steam_client.AdminComponents.Templates
{
    /// <summary>
    /// Логика взаимодействия для UserDBComponent.xaml
    /// </summary>
    public partial class UserDBComponent : UserControl
    {
        public UserDBComponent()
        {
            InitializeComponent();
            DataContext = new UserViewModel();
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
