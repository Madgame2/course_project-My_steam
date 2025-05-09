﻿using System;
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
    /// Логика взаимодействия для DownLoadInfo.xaml
    /// </summary>
    public partial class DownLoadInfo : UserControl
    {

        public long InstallSizeBytes { get; set; } = 0;
        public DownLoadInfo(long spaceRequred)
        {
            InstallSizeBytes=spaceRequred;

            InitializeComponent();
            DataContext = this;
        }
    }
}
