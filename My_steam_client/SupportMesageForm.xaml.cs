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
using Game_Net_DTOLib;
using System.Windows.Shapes;
using My_steam_client.Scripts;
using Game_Net;
using Microsoft.Extensions.DependencyInjection;

namespace My_steam_client
{
    /// <summary>
    /// Логика взаимодействия для SupportMesageForm.xaml
    /// </summary>
    public partial class SupportMesageForm : Window
    {
        ReportsService service;
        public SupportMesageForm()
        {
            InitializeComponent();
            HeaderContaner.Content = new Header(this,"Support message form");

            service = AppServices.Provider.GetRequiredService<ReportsService>();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var report = new SendReportDTO
            {
                UserId = AppServices.UserId.ToString(),
                Message = TextPlace.Text
            };

            switch (MyComboBox.SelectedIndex)
            {
                case 0:
                    report.Topic = ReportToppic.Complaint;
                    break;
                case 1:
                    report.Topic = ReportToppic.ReQuest_for_publisher;
                    break;
            }

            try
            {
                await service.SendMessage(report);
                MessageBox.Show("Secsessfuly sended");
            }
            catch (Exception ex)
            {
                
               MessageBox.Show(ex.Message);
                
            }
        }
    }
}
