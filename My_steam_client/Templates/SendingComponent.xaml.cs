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
using System.Windows.Threading;
using WpfAnimatedGif;

namespace My_steam_client.Templates
{
    /// <summary>
    /// Логика взаимодействия для SendingComponent.xaml
    /// </summary>
    public partial class SendingComponent : UserControl
    {
        private DispatcherTimer _timer;
        private int _dotCount = 0;

        private string defoultTilte = "";

        public SendingComponent()
        {
            InitializeComponent();

            defoultTilte = LoadingTextBlock.Text;

            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("pack://application:,,,/resources/Gif/Loading.gif", UriKind.Absolute);
            image.EndInit();
            ImageBehavior.SetAnimatedSource(GifImage, image);


            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _dotCount = (_dotCount + 1) % 4; // 0 to 3
            LoadingTextBlock.Text = defoultTilte + new string('.', _dotCount);
        }
    }
}
