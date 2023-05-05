using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AuthLogin.xaml
    /// </summary>
    public partial class AuthLogin : Window
    {
        bool passwordHide = false;
        MainWindow MainWindow;
        public AuthLogin(MainWindow main)
        {
            ResourceDictionary resourceDict = new ResourceDictionary();
            // Добавляем в него ресурсы из внешнего файла
            resourceDict.MergedDictionaries.Clear();
            resourceDict.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"Dictionary{ObjectJsonStatic.load}.xaml", UriKind.RelativeOrAbsolute) });
            // Устанавливаем ресурсный словарь как ресурсы окна
            this.Resources = resourceDict;
            InitializeComponent();
            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), ObjectJsonStatic.allDizain[ObjectJsonStatic.load][$"{ObjectJsonStatic.load}"][1])));
            MainWindow = main;
            Login.Text = ObjectJsonStatic.login;
            Password.Text = ObjectJsonStatic.password;
        }
        public void RemoveText(object sender, EventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (instance.Text == instance.Tag.ToString())
                instance.Text = "";
        }

        public void AddText(object sender, EventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(instance.Text))
                instance.Text = instance.Tag.ToString();
        }
        private void BackApp_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Show();
            this.Close();
        }

        private void savePassLog()
        {
            if (ObjectJsonStatic.password != Password.Text && ObjectJsonStatic.login != Login.Text)
            {
                ObjectJson objectJson = new ObjectJson();
                objectJson.allDizain = ObjectJsonStatic.allDizain;
                objectJson.load = ObjectJsonStatic.load;
                objectJson.password = Password.Text;
                objectJson.login = Login.Text;
                var json = JsonConvert.SerializeObject(objectJson);
                File.WriteAllText("jsconfig1.json", json);
            }
        }

        private void InputApp_Click(object sender, RoutedEventArgs e)
        {

            if (Login.Text == "nv1603a" && Password.Text == "fkebm2")
            {
                savePassLog();
                MainDB maindb = new MainDB(1);
                maindb.Show();
                this.Close();
            }
            else if (Login.Text == "1" && Password.Text == "2")
            {
                savePassLog();
                MainDB maindb = new MainDB(2);
                maindb.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Вы ввели не правильный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void HideOpenPassword(object sender, MouseButtonEventArgs e)
        {
            passwordHide = !passwordHide;
            if (passwordHide)
            {
                Password.FontSize = 0.1;
            }
            else
            {
                Password.FontSize = 72;
            }

        }
    }
}
