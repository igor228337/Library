using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
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
using System.Windows.Shapes;
using System.Runtime.Remoting.Messaging;
using System.Windows.Markup;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        string[] languageEn = new string[] { "List_of_books", "Title", "Author", "Series", "Genre", "Publishing_house", "Year_of_publication", "Number_of_pages", "Rating", "Reading_status", "Start_date", "Final_date"};
        string[] languageRu = new string[] { "Список книг", "Название", "Автор", "Серия", "Жанр", "Издатель", "Год публикации", "Количество страниц", "Рейтинг", "Статус прочтения", "Дата начала", "Дата конца" };
        Dictionary<string, string> langRuEn = new Dictionary<string, string>();
        readonly Dictionary<string, string> BDTable = new Dictionary<string, string>()
        {
            ["List_of_books"] = "Title,Author,Series,Genre,Publishing_house,Year_of_publication,Number_of_pages,Rating,Reading_status,Start_date,Final_date",
            ["Author"] = "Author",
            ["Series"] = "Series",
            ["Genre"] = "Genre",
            ["Reading_status"] = "Reading_status",
            ["Rating"] = "Rating"
        };
        List<Author> listAuthors = new List<Author>();
        List<Series> listSeries = new List<Series>();
        List<Genre> listGenre = new List<Genre>();
        List<ReadingStatus> listReadingStatus = new List<ReadingStatus>();
        List<object[]> listListOfBooks = new List<object[]>();
        List<ListOfBooks> listListOfBooksGood = new List<ListOfBooks>();
        List<ListOfBooksP> listListOfBooksGoodP = new List<ListOfBooksP>();
        List<Rating> listRating = new List<Rating>();

        string NameDB { get; set; }
        public Search(List<string> tables, string nameDB)
        {
            List<string> table = new List<string>();
            InitializeComponent();
            langRuEn = languageEn.Zip(languageRu, (k, v) => new { Key = k, Value = v })
                                             .ToDictionary(x => x.Key, x => x.Value);
            foreach (string tableName in tables)
            {
                string dataName = translate(tableName);
                table.Add(dataName);
            }
            ComboVombo.ItemsSource = table;
            NameDB = nameDB;
        }

        private string translate(string word)
        {
            string value;
            if (langRuEn.ContainsKey(word))
            {
                value = langRuEn[word];
            }
            else
            {
                value = langRuEn.FirstOrDefault(x => x.Value == word).Key;
            }
            return value;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> itemList = new List<string>();
            string item = BDTable[translate(ComboVombo.SelectedItem.ToString())];
            foreach (string value in item.Split(','))
            {
                if (value == "Rating" && NameDB == "Data Source=./LibraryPanasuk.db")
                {
                    continue;
                }
                itemList.Add(translate(value));
            }
            ComboVombo2.ItemsSource = itemList;
        }
        private string getDataDB(string cmd)
        {
            string data = "";
            using (var connection = new SQLiteConnection(NameDB))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(cmd, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        data = reader.GetString(0).ToString();
                    }
                }
                connection.Close();
            }
            return data;
        }
        private void ReadBDBooks(string value, string valueColumn)
        {
            List<string> dataId = new List<string>();
            string cmd = $"SELECT * FROM {valueColumn} WHERE {valueColumn} LIKE '{SearchTextBox.Text}%'";
            string cmd2 = $"SELECT * FROM {value} WHERE {valueColumn}=";
            using (var connection = new SQLiteConnection(NameDB))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(cmd, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string gg = reader.GetValue(0).ToString();
                        dataId.Add(gg);
                    }
                }
                foreach (string item in dataId)
                {
                    SQLiteCommand command2 = new SQLiteCommand(cmd2 + item, connection);
                    using (SQLiteDataReader reader = command2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object id = reader.GetValue(0);
                            object title = reader.GetValue(1);
                            object author = reader.GetValue(2);
                            object series = reader.GetValue(3);
                            object genre = reader.GetValue(4);
                            object publishingHouse = reader.GetValue(5);
                            object Year_of_publication = reader.GetValue(6);
                            object Number_of_pages = reader.GetValue(7);
                            if (NameDB == "Data Source=./LibraryPanasuk.db")
                            {
                                object Reading_status = reader.GetValue(8);
                                object Start_date = reader.GetValue(9);
                                object Final_date = reader.GetValue(10);
                                object[] listObject = new object[] { id, title, author, series, genre, publishingHouse, Year_of_publication, Number_of_pages, Reading_status, Start_date, Final_date };
                                listListOfBooks.Add(listObject);
                            }
                            else
                            {
                                object Rating = reader.GetValue(8);
                                object Reading_status = reader.GetValue(9);
                                object Start_date = reader.GetValue(10);
                                object Final_date = reader.GetValue(11);
                                object[] listObject = new object[] { id, title, author, series, genre, publishingHouse, Year_of_publication, Number_of_pages, Rating, Reading_status, Start_date, Final_date };
                                listListOfBooks.Add(listObject);
                            }
                            
                        }
                    }
                }
                
                connection.Close();
            }
        }
        private void WriteDataGrid(string cmd)
        {
            listAuthors.Clear();
            listSeries.Clear();
            listGenre.Clear();
            listReadingStatus.Clear();
            listListOfBooksGood.Clear();
            listListOfBooksGoodP.Clear();
            listRating.Clear();
            listListOfBooks.Clear();
            string value = translate(ComboVombo.SelectedItem.ToString());
            string valueColumn = translate(ComboVombo2.SelectedItem.ToString());
            if (value == "List_of_books" && (valueColumn == "Author" || valueColumn == "Series" || valueColumn == "Genre" || valueColumn == "Rating" || valueColumn == "Reading_status"))
            {
                ReadBDBooks(value, valueColumn);
            }
            else
            {
                ReadBD(cmd);
            }
            
            if (listAuthors.Count != 0)
            {
                SearchGrid.ItemsSource = null;
                SearchGrid.Items.Refresh();
                SearchGrid.ItemsSource = listAuthors;
                SearchGrid.Items.Refresh();
            }
            else if (listSeries.Count != 0)
            {
                SearchGrid.ItemsSource = null;
                SearchGrid.Items.Refresh();
                SearchGrid.ItemsSource = listSeries;
                SearchGrid.Items.Refresh();
            }
            else if (listGenre.Count != 0)
            {
                SearchGrid.ItemsSource = null;
                SearchGrid.Items.Refresh();
                SearchGrid.ItemsSource = listGenre;
                SearchGrid.Items.Refresh();
            }
            else if (listReadingStatus.Count != 0)
            {
                SearchGrid.ItemsSource = null;
                SearchGrid.Items.Refresh();
                SearchGrid.ItemsSource = listReadingStatus;
                SearchGrid.Items.Refresh();
            }
            else if (listListOfBooks.Count != 0)
            {
                SearchGrid.ItemsSource = null;
                SearchGrid.Items.Refresh();
                for (int i = 0; i < listListOfBooks.Count; i++)
                {
                    object[] tempBooks = listListOfBooks[i];
                    long id = long.Parse(tempBooks[0].ToString());
                    if (tempBooks[0].ToString() == null && tempBooks[0].ToString() == "")
                    {
                        continue;
                    }
                    string title = tempBooks[1].ToString();
                    string author = tempBooks[2].ToString(); ;
                    if (author != null && author != "")
                    {
                        author = getDataDB($"SELECT Author FROM Author WHERE id={author}");
                    }
                    string series = tempBooks[3].ToString(); ;
                    if (series != null && series != "")
                    {
                        series = getDataDB($"SELECT Series FROM Series WHERE id={series}");
                    }
                    string genre = tempBooks[4].ToString(); ;
                    if (genre != null && genre != "")
                    {
                        genre = getDataDB($"SELECT Genre FROM Genre WHERE id={genre}");
                    }
                    
                    
                    string publishingHouse = tempBooks[5].ToString();
                    string Year_of_publication = tempBooks[6].ToString();
                    long Number_of_pages = long.Parse(tempBooks[7].ToString());
                    if (NameDB == "Data Source=./LibraryPanasuk.db")
                    {
                        string Reading_status = tempBooks[8].ToString();
                        if (Reading_status != null && Reading_status != "")
                        {
                            Reading_status = getDataDB($"SELECT Reading_status FROM Reading_status WHERE id={Reading_status}");
                        }
                        string Start_date = tempBooks[9].ToString();
                        string Final_date = tempBooks[10].ToString();
                        listListOfBooksGoodP.Add(new ListOfBooksP(id, title, author, series, genre, publishingHouse, Year_of_publication, Number_of_pages, Reading_status, Start_date, Final_date));

                    }
                    else
                    {
                        var isNumeric = long.TryParse(tempBooks[8].ToString(), out long rat);
                        string rating;
                        if (isNumeric)
                        {
                            rating = getDataDB($"SELECT Rating FROM Rating WHERE id={rat}");
                        }
                        else
                        {
                            rating = tempBooks[8].ToString();
                        }

                        string Reading_status = tempBooks[9].ToString();
                        if (Reading_status != null && Reading_status != "")
                        {
                            Reading_status = getDataDB($"SELECT Reading_status FROM Reading_status WHERE id={Reading_status}");
                        }
                        string Start_date = tempBooks[10].ToString();
                        string Final_date = tempBooks[11].ToString();
                        listListOfBooksGood.Add(new ListOfBooks(id, title, author, series, genre, publishingHouse, Year_of_publication, Number_of_pages, rating, Reading_status, Start_date, Final_date));

                    }

                }
                listListOfBooks.Clear();
                if (NameDB == "Data Source=./LibraryPanasuk.db")
                {
                    SearchGrid.ItemsSource = listListOfBooksGoodP;
                }
                else
                {
                    SearchGrid.ItemsSource = listListOfBooksGood;
                }
                
                SearchGrid.Items.Refresh();
            }
            else if (listRating.Count != 0)
            {
                SearchGrid.ItemsSource = null;
                SearchGrid.Items.Refresh();
                SearchGrid.ItemsSource = listRating;
                SearchGrid.Items.Refresh();
            }
            
        }
        private void ReadBD(string cmd)
        {
            using (var connection = new SQLiteConnection(NameDB))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(cmd, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) 
                    {
                        int countColums = reader.FieldCount;
                        if (countColums <= 3)
                        {
                            object id = reader.GetValue(0);
                            long id1 = long.Parse(id.ToString());
                            object value = reader.GetValue(1);
                            switch (translate(ComboVombo.SelectedItem.ToString()))
                            {
                                case "Author":
                                    Author stringsAuthor = new Author(id1, value.ToString());
                                    listAuthors.Add(stringsAuthor);
                                    break;
                                case "Series":
                                    Series stringsSeries = new Series(id1, value.ToString());
                                    listSeries.Add(stringsSeries);
                                    break;
                                case "Genre":
                                    Genre stringsGenre = new Genre(id1, value.ToString());
                                    listGenre.Add(stringsGenre);
                                    break;
                                case "Reading_status":
                                    ReadingStatus stringsReadingStatus = new ReadingStatus(id1, value.ToString());
                                    listReadingStatus.Add(stringsReadingStatus);
                                    break;
                                case "Rating":
                                    Rating stringsRating = new Rating(id1, value.ToString());
                                    listRating.Add(stringsRating);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            if (NameDB == "Data Source=./LibraryPanasuk.db")
                            {
                                object id = reader.GetValue(0);
                                object title = reader.GetValue(1);
                                object author = reader.GetValue(2);
                                object series = reader.GetValue(3);
                                object genre = reader.GetValue(4);
                                object publishingHouse = reader.GetValue(5);
                                object Year_of_publication = reader.GetValue(6);
                                object Number_of_pages = reader.GetValue(7);
                                object Reading_status = reader.GetValue(8);
                                object Start_date = reader.GetValue(9);
                                object Final_date = reader.GetValue(10);
                                object[] listObject = new object[] { id, title, author, series, genre, publishingHouse, Year_of_publication, Number_of_pages, Reading_status, Start_date, Final_date };
                                listListOfBooks.Add(listObject);
                            }
                            else
                            {
                                object id = reader.GetValue(0);
                                object title = reader.GetValue(1);
                                object author = reader.GetValue(2);
                                object series = reader.GetValue(3);
                                object genre = reader.GetValue(4);
                                object publishingHouse = reader.GetValue(5);
                                object Year_of_publication = reader.GetValue(6);
                                object Number_of_pages = reader.GetValue(7);
                                object Rating = reader.GetValue(8);
                                object Reading_status = reader.GetValue(9);
                                object Start_date = reader.GetValue(10);
                                object Final_date = reader.GetValue(11);
                                object[] listObject = new object[] { id, title, author, series, genre, publishingHouse, Year_of_publication, Number_of_pages, Rating, Reading_status, Start_date, Final_date };
                                listListOfBooks.Add(listObject);
                            }
                            
                        }
                    }
                }
                connection.Close();
            }
            
        }
        private void SearchGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SearchTextBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string value;
            string valueColumn;
            try
            {
                value = translate(ComboVombo.SelectedItem.ToString());
                valueColumn = translate(ComboVombo2.SelectedItem.ToString());
            }
            catch(NullReferenceException) 
            {
                return;
            }
            string text = SearchTextBox.Text;
            if (text != "" && text != null)
            {
                string cmd = $"SELECT * FROM {value}" + "\n" + $"WHERE {valueColumn} LIKE '{text}%'";
                WriteDataGrid(cmd);
            }
            else { SearchGrid.ItemsSource = null; }
            
        }

        private void ComboBox_SelectionChanged2(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Clear();
            SearchGrid.ItemsSource = null;

        }
    }
}
