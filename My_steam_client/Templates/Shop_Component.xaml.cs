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
    /// Логика взаимодействия для Shop_Component.xaml
    /// </summary>
    public partial class Shop_Component : UserControl
    {
        private MainWindow _mainWindow;

        public Shop_Component(MainWindow window)
        {
            InitializeComponent();
            _mainWindow = window;

            showCase.addObject(new ShowCaseObject());
            showCase.addObject(new ShowCaseObject());
        }
    }
}
