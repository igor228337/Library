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

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для Razrab.xaml
    /// </summary>
    public partial class Razrab : Window
    {
        MainDB Maindb { get; set; }
        public Razrab(MainDB maindb)
        {
            ResourceDictionary resourceDict = new ResourceDictionary();
            // Добавляем в него ресурсы из внешнего файла
            resourceDict.MergedDictionaries.Clear();
            resourceDict.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"Dictionary{ObjectJsonStatic.load}.xaml", UriKind.RelativeOrAbsolute) });
            // Устанавливаем ресурсный словарь как ресурсы окна
            this.Resources = resourceDict;
            InitializeComponent();
            this.Maindb = maindb;
            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), ObjectJsonStatic.allDizain[ObjectJsonStatic.load][$"{ObjectJsonStatic.load}"][3])));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Maindb.Show();
            this.Close();
        }

        private void NextPage_click(object sender, RoutedEventArgs e)
        {
            Tester test = new Tester(Maindb);
            test.Show();
            this.Close();
        }

        private void AboutApp_Click(object sender, RoutedEventArgs e)
        {
            AboutApp aboutApp = new AboutApp(Maindb);
            aboutApp.Show();
            this.Close();
        }

        private void Team_Button(object sender, RoutedEventArgs e)
        {

        }

        private void CatTeam_Botton(object sender, RoutedEventArgs e)
        {
            CatCommand catCommand = new CatCommand(Maindb);
            catCommand.Show();
            this.Close();
        }

        private void Settings_Botton(object sender, RoutedEventArgs e)
        {
            DefaultSettings defaultSettings = new DefaultSettings(Maindb);
            defaultSettings.Show();
            this.Close();
        }

        private void BackPage_click(object sender, RoutedEventArgs e)
        {
            Diz Diz = new Diz(Maindb);
            Diz.Show();
            this.Close();
        }
    }
}
