using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
using Newtonsoft.Json;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            string json = File.ReadAllText("jsconfig1.json");
            ObjectJson data = JsonConvert.DeserializeObject<ObjectJson>(json);
            ObjectJsonStatic.allDizain = data.allDizain;
            ObjectJsonStatic.load = data.load;
            ObjectJsonStatic.login = data.login;
            ObjectJsonStatic.password = data.password;
            ResourceDictionary resourceDict = new ResourceDictionary();

            // Добавляем в него ресурсы из внешнего файла
            resourceDict.MergedDictionaries.Clear();
            resourceDict.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"Dictionary{ObjectJsonStatic.load}.xaml", UriKind.RelativeOrAbsolute) });
            // Устанавливаем ресурсный словарь как ресурсы окна
            this.Resources = resourceDict;
            InitializeComponent();
            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), ObjectJsonStatic.allDizain[ObjectJsonStatic.load][$"{ObjectJsonStatic.load}"][0])));
        }

        private void OutApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Input_Click(object sender, RoutedEventArgs e)
        {
            AuthLogin auth = new AuthLogin(this);
            auth.Show();
            this.Hide();
        }
    }
}
