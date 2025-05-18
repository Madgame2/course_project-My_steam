using System.Windows.Controls;
using My_steam_client.AdminComponents.ModelsView;

namespace My_steam_client.AdminComponents.Templates
{
    /// <summary>
    /// Логика взаимодействия для GoodsDBComponent.xaml
    /// </summary>
    public partial class GoodsDBComponent : UserControl
    {
        public GoodsDBComponent()
        {
            InitializeComponent();
            DataContext = new GamesViewModel();
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
