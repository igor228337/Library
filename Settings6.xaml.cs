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
    /// Логика взаимодействия для Settings6.xaml
    /// </summary>
    public partial class Settings6 : Window
    {
        MainDB Maindb { get; set; }
        public Settings6(MainDB maindb)
        {
            ResourceDictionary resourceDict = new ResourceDictionary();
            // Добавляем в него ресурсы из внешнего файла
            resourceDict.MergedDictionaries.Clear();
            resourceDict.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"Dictionary{ObjectJsonStatic.load}.xaml", UriKind.RelativeOrAbsolute) });
            // Устанавливаем ресурсный словарь как ресурсы окна
            this.Resources = resourceDict;
            InitializeComponent();
            if (ObjectJsonStatic.load == 5)
            {
                Choice_Style.Opacity = 0.5;
                Choice_Style.Content = "Выбрано";
                Choice_Style.IsEnabled = false;
            }
            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), ObjectJsonStatic.allDizain[ObjectJsonStatic.load][$"{ObjectJsonStatic.load}"][3])));
            this.Maindb = maindb;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Maindb.Show();
            this.Close();
        }

        private void NextPage_click(object sender, RoutedEventArgs e)
        {
            Settings7 settings = new Settings7(Maindb);
            settings.Show();
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
            TeamLid teamLid = new TeamLid(Maindb);
            teamLid.Show();
            this.Close();
        }

        private void CatTeam_Botton(object sender, RoutedEventArgs e)
        {
            CatCommand catCommand = new CatCommand(Maindb);
            catCommand.Show();
            this.Close();
        }

        private void Settings_Botton(object sender, RoutedEventArgs e)
        {

        }

        private void Choice_Style1(object sender, RoutedEventArgs e)
        {
            ObjectJsonStatic.load = 5;
            ObjectJson objectJson = new ObjectJson();
            objectJson.allDizain = ObjectJsonStatic.allDizain;
            objectJson.load = ObjectJsonStatic.load;
            var json = JsonConvert.SerializeObject(objectJson);
            File.WriteAllText("jsconfig1.json", json);
            Choice_Style.Opacity = 0.5;
            Choice_Style.Content = "Выбрано";
            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), ObjectJsonStatic.allDizain[ObjectJsonStatic.load][$"{ObjectJsonStatic.load}"][3])));
            int temp = Maindb.NameDBInt;
            Maindb = new MainDB(temp);
            Settings6 settings6 = new Settings6(Maindb);
            settings6.Show();
            this.Close();
        }

        private void BackPage_click(object sender, RoutedEventArgs e)
        {
            Settings5 settings = new Settings5(Maindb);
            settings.Show();
            this.Close();
        }
    }
}
