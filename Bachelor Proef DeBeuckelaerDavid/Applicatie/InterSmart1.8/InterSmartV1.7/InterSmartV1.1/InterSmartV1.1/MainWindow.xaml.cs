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

namespace InterSmartV1._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
           
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            InterSmartV1._1.Menu.MainPage WinMain = new Menu.MainPage();
            WinMain.Show();
            this.Hide();
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    InterSmartV1._1.Menu.Options WinOption = new Menu.Options();
        //    WinOption.Show();
        //}
    }
}
