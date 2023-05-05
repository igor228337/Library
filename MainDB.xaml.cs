using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data.Entity.Infrastructure;
using System.Windows.Navigation;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Xml;



namespace Library
{
    /// <summary>
    /// Логика взаимодействия для MainDB.xaml
    /// </summary>
    public partial class MainDB : Window
    {
        List<Button> buttonsBD = new List<Button>();
        string sqlquere = "";
        string sqlAnother = "SELECT * FROM ";
        string sqlSeries = "SELECT s.id, s.Series, a.Author FROM Series s\r\nJOIN Author a ON s.id_author = a.id;";
        public string NameDB { get; set; }
        public int NameDBInt { get; set; }
        List<string> tables = new List<string>();
        public MainDB(int nameDB)
        {
            ResourceDictionary resourceDict = new ResourceDictionary();
            // Добавляем в него ресурсы из внешнего файла
            resourceDict.MergedDictionaries.Clear();
            resourceDict.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"Dictionary{ObjectJsonStatic.load}.xaml", UriKind.RelativeOrAbsolute) });
            // Устанавливаем ресурсный словарь как ресурсы окна
            this.Resources = resourceDict;
            InitializeComponent();
            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), ObjectJsonStatic.allDizain[ObjectJsonStatic.load][$"{ObjectJsonStatic.load}"][2])));
            NameDBInt = nameDB;
            if (nameDB == 1)
            {
                this.buttonsBD = new List<Button>() { books, Authors, Fors, Genre, ReadingStatus, Rating };
                NameDB = "Data Source=./LibraryVershinina.db";
                tables = new List<string>() { "List_of_books", "Author", "Series", "Genre", "Reading_status", "Rating" };
                MainGrid.FontSize = 20;
                sqlquere = "SELECT List_of_books.№, List_of_books.Title, Author.Author AS Author, Series.Series AS Series, Genre.Genre AS Genre,\r\n       List_of_books.Publishing_house, List_of_books.Year_of_publication, List_of_books.Number_of_pages, \r\n       Rating.Rating AS Rating, Reading_status.Reading_status AS Reading_status,\r\n       List_of_books.Start_date, List_of_books.Final_date\r\nFROM List_of_books\r\nLEFT JOIN Author ON List_of_books.Author = Author.id\r\nLEFT JOIN Series ON List_of_books.Series = Series.id\r\nLEFT JOIN Genre ON List_of_books.Genre = Genre.id\r\nLEFT JOIN Rating ON List_of_books.Rating = Rating.id\r\nLEFT JOIN Reading_status ON List_of_books.Reading_status = Reading_status.id;";
            }
            else
            {
                NameDB = "Data Source=./LibraryPanasuk.db";
                this.buttonsBD = new List<Button>() { books, Authors, Fors, Genre, ReadingStatus};
                tables = new List<string>() { "List_of_books", "Author", "Series", "Genre", "Reading_status"};
                Rating.Opacity = 0;
                sqlquere = "SELECT List_of_books.№, List_of_books.Title, Author.Author AS Author, Series.Series AS Series, Genre.Genre AS Genre,\r\n       List_of_books.Publishing_house, List_of_books.Year_of_publication, List_of_books.Number_of_pages, \r\n       Reading_status.Reading_status AS Reading_status,\r\n       List_of_books.Start_date, List_of_books.Final_date\r\nFROM List_of_books\r\nLEFT JOIN Author ON List_of_books.Author = Author.id\r\nLEFT JOIN Series ON List_of_books.Series = Series.id\r\nLEFT JOIN Genre ON List_of_books.Genre = Genre.id\r\nLEFT JOIN Reading_status ON List_of_books.Reading_status = Reading_status.id;";
            }
            WriteDataGrid(sqlquere);
            
        }
        private void WriteDataGrid(string cmd)
        {
            using (var connection = new SQLiteConnection(this.NameDB))
            {
                connection.Open();
                SQLiteCommand createCommand = new SQLiteCommand(cmd, connection);
                createCommand.ExecuteNonQuery();
                SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(createCommand);
                DataTable dt = new DataTable();
                dataAdp.Fill(dt);
                MainGrid.ItemsSource = dt.DefaultView;
                connection.Close();

            }
        }
        private void ClearButton()
        {
            foreach (Button item in this.buttonsBD)
            {
                item.Opacity = 0.6;
            }
        }
        private void AboutApp_Click(object sender, RoutedEventArgs e)
        {
            AboutApp aboutApp = new AboutApp(this);
            aboutApp.Show();
            this.Hide();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }

        private void AllSearch_Button(object sender, RoutedEventArgs e)
        {

        }
        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            int id;
            string idStr;
            SQLiteCommand commandDelete;
            List<int> ids = new List<int>();
            List<string> idsName = new List<string>();
            List<DataRow> rows = new List<DataRow>();
            int tablesTemp = findActiveButton();
            if (e.Key == Key.Delete)
            {
                foreach (object item in MainGrid.SelectedItems)
                {
                    DataRowView rowView = item as DataRowView;
                    if (rowView != null)
                    {
                        DataRow row = rowView.Row;
                        if (tablesTemp == 0)
                        {
                            id = Convert.ToInt32(row["№"]);
                            idStr = "№";
                        }
                        else
                        {
                            id = Convert.ToInt32(row["id"]);
                            idStr = "id";
                        }
                        ids.Add(id);
                        idsName.Add(idStr);
                        rows.Add(row);
                    }
                }
                using (var connection = new SQLiteConnection(this.NameDB))
                {
                    connection.Open();
                    for (int i = 0; i < ids.Count; i++)
                    {
                        string command = $"DELETE FROM {tables[tablesTemp]} WHERE {idsName[i]}={ids[i]}";
                        commandDelete = new SQLiteCommand(command, connection);
                        commandDelete.ExecuteNonQuery();
                        rows[i].Delete();
                    }
                    connection.Close();
                }
            }
            

        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            int tablesTemp = findActiveButton();
            DataTable dataTable = ((DataView)MainGrid.ItemsSource).Table;

            using (var connection = new SQLiteConnection(this.NameDB))
            {
                connection.Open();
                foreach (DataRow row in dataTable.Rows)
                {
                    int id;
                    string idStr;
                    try
                    {
                        if (tablesTemp == 0)
                        {
                            id = Convert.ToInt32(row["№"]);
                            idStr = "№";
                        }
                        else
                        {
                            id = Convert.ToInt32(row["id"]);
                            idStr = "id";
                        }
                    }
                    catch (DeletedRowInaccessibleException)
                    {

                        return;
                    }
                    SQLiteCommand selectCommandFind = new SQLiteCommand($"SELECT * FROM {tables[tablesTemp]} WHERE {idStr}={id}", connection);
                    SQLiteDataReader reader = selectCommandFind.ExecuteReader();
                    if (reader.HasRows)
                    {
                        for (int i = 1; i < row.ItemArray.Length; i++)
                        {
                            if (tables.Count == 5)
                            {
                                // Перебор листа книги
                                if (tablesTemp == 0)
                                {
                                    switch (i)
                                    {
                                        case 2:
                                            string author = row.ItemArray[i].ToString();
                                            SQLiteCommand selectCommandFind2 = new SQLiteCommand($"SELECT id FROM Author WHERE Author='{author}'", connection);
                                            SQLiteDataReader reader2 = selectCommandFind2.ExecuteReader();
                                            if (reader2.HasRows)
                                            {
                                                if (reader2.Read())
                                                {
                                                    UpdateDataId(tables[tablesTemp], "Author", $"{reader2.GetInt32(0)}", idStr, id, connection);
                                                }
                                            }
                                            else
                                            {
                                                string columns = "(Author)";
                                                InsertData("Author", connection, columns, $"('{author}')");
                                                int idLastPlusOne = GetLastId("Author", connection);
                                                UpdateDataId(tables[tablesTemp], "Author", $"{idLastPlusOne}", idStr, id, connection);
                                            }
                                            break;
                                        case 3:
                                            string series = row.ItemArray[i].ToString();
                                            SQLiteCommand selectCommandFind5 = new SQLiteCommand($"SELECT id FROM Series WHERE Series='{series}'", connection);
                                            SQLiteDataReader reader5 = selectCommandFind5.ExecuteReader();
                                            if (reader5.HasRows)
                                            {
                                                if (reader5.Read())
                                                {
                                                    UpdateDataId(tables[tablesTemp], "Series", $"{reader5.GetInt32(0)}", idStr, id, connection);
                                                }
                                            }
                                            else
                                            {
                                                string columns = "(Series, id_author)";
                                                int id_author;
                                                SQLiteCommand selectCommandFind6 = new SQLiteCommand($"SELECT id FROM Author WHERE Author='{row.ItemArray[2]}'", connection);
                                                SQLiteDataReader reader6 = selectCommandFind6.ExecuteReader();
                                                if (reader6.HasRows)
                                                {
                                                    if (reader6.Read())
                                                    {
                                                        id_author = reader6.GetInt32(0);
                                                        InsertData("Series", connection, columns, $"('{series}', {id_author})");
                                                        int idLastPlusOne = GetLastId("Series", connection);
                                                        UpdateDataId(tables[tablesTemp], "Series", $"{idLastPlusOne}", idStr, id, connection);
                                                    }
                                                }
                                                else
                                                {
                                                    string columns2 = "(Author)";
                                                    InsertData("Author", connection, columns2, $"('{row.ItemArray[2]}')");
                                                    int idLastPlusOne2 = GetLastId("Author", connection);
                                                    InsertData("Series", connection, columns, $"('{series}', {idLastPlusOne2})");
                                                    int idLastPlusOne = GetLastId("Series", connection);
                                                    UpdateDataId(tables[tablesTemp], "Series", $"{idLastPlusOne}", idStr, id, connection);
                                                }

                                            }
                                            break;
                                        case 4:
                                            string genry = row.ItemArray[i].ToString();
                                            SQLiteCommand selectCommandFind3 = new SQLiteCommand($"SELECT id FROM Genre WHERE Genre='{genry}'", connection);
                                            SQLiteDataReader reader3 = selectCommandFind3.ExecuteReader();
                                            if (reader3.HasRows)
                                            {
                                                if (reader3.Read())
                                                {
                                                    UpdateDataId(tables[tablesTemp], "Genre", $"{reader3.GetInt32(0)}", idStr, id, connection);
                                                }
                                            }
                                            else
                                            {
                                                string columns = "(Genre)";
                                                InsertData("Genre", connection, columns, $"('{genry}')");
                                                int idLastPlusOne = GetLastId("Genre", connection);
                                                UpdateDataId(tables[tablesTemp], "Genre", $"{idLastPlusOne}", idStr, id, connection);
                                            }
                                            break;
                                        case 8:
                                            string status = row.ItemArray[i].ToString();
                                            SQLiteCommand selectCommandFind4 = new SQLiteCommand($"SELECT id FROM Reading_status WHERE Reading_status='{status}'", connection);
                                            SQLiteDataReader reader4 = selectCommandFind4.ExecuteReader();
                                            if (reader4.HasRows)
                                            {
                                                if (reader4.Read())
                                                {
                                                    UpdateDataId(tables[tablesTemp], "Reading_status", $"{reader4.GetInt32(0)}", idStr, id, connection);
                                                }
                                            }
                                            else
                                            {
                                                string columns = "(Reading_status)";
                                                InsertData("Reading_status", connection, columns, $"('{status}')");
                                                int idLastPlusOne = GetLastId("Reading_status", connection);
                                                UpdateDataId(tables[tablesTemp], "Reading_status", $"{idLastPlusOne}", idStr, id, connection);
                                            }
                                            break;
                                        default:
                                            UpdateData(row, i, tablesTemp, idStr, id, connection);
                                            break;
                                    }
                                }
                                // Перебор серии
                                else if (tablesTemp == 2)
                                {
                                    switch (i)
                                    {
                                        case 2:
                                            SQLiteCommand selectCommandFind2 = new SQLiteCommand($"SELECT id FROM Author WHERE Author='{row.ItemArray[i]}'", connection);
                                            SQLiteDataReader reader2 = selectCommandFind2.ExecuteReader();
                                            if (reader2.HasRows)
                                            {
                                                if (reader2.Read())
                                                {
                                                    UpdateDataId(tables[tablesTemp], "id_author", $"{reader2.GetInt32(0)}", idStr, id, connection);
                                                }
                                            }
                                            else
                                            {
                                                string columns = "(Author)";
                                                InsertData("Author", connection, columns, $"('{row.ItemArray[i]}')");
                                                int idLastPlusOne = GetLastId("Author", connection);
                                                UpdateDataId(tables[tablesTemp], "id_author", $"{idLastPlusOne}", idStr, id, connection);
                                            }
                                            break;

                                        default:
                                            UpdateData(row, i, tablesTemp, idStr, id, connection);
                                            break;
                                    }
                                }
                                // Остальные случаи
                                else
                                {
                                    UpdateData(row, i, tablesTemp, idStr, id, connection);
                                }
                            }
                            else
                            {
                                if (tablesTemp == 0)
                                {
                                    // Перебор книг
                                    switch (i)
                                    {
                                        case 2:
                                            string author = row.ItemArray[i].ToString();
                                            SQLiteCommand selectCommandFind2 = new SQLiteCommand($"SELECT id FROM Author WHERE Author='{author}'", connection);
                                            SQLiteDataReader reader2 = selectCommandFind2.ExecuteReader();
                                            if (reader2.HasRows)
                                            {
                                                if (reader2.Read())
                                                {
                                                    UpdateDataId(tables[tablesTemp], "Author", $"{reader2.GetInt32(0)}", idStr, id, connection);
                                                }
                                            }
                                            else
                                            {
                                                string columns = "(Author)";
                                                InsertData("Author", connection, columns, $"('{author}')");
                                                int idLastPlusOne = GetLastId("Author", connection);
                                                UpdateDataId(tables[tablesTemp], "Author", $"{idLastPlusOne}", idStr, id, connection);
                                            }
                                            break;
                                        case 3:
                                            string series = row.ItemArray[i].ToString();
                                            SQLiteCommand selectCommandFind5 = new SQLiteCommand($"SELECT id FROM Series WHERE Series='{series}'", connection);
                                            SQLiteDataReader reader5 = selectCommandFind5.ExecuteReader();
                                            if (reader5.HasRows)
                                            {
                                                if (reader5.Read())
                                                {
                                                    UpdateDataId(tables[tablesTemp], "Series", $"{reader5.GetInt32(0)}", idStr, id, connection);
                                                }
                                            }
                                            else
                                            {
                                                string columns = "(Series, id_author)";
                                                int id_author;
                                                SQLiteCommand selectCommandFind6 = new SQLiteCommand($"SELECT id FROM Author WHERE Author='{row.ItemArray[2]}'", connection);
                                                SQLiteDataReader reader6 = selectCommandFind6.ExecuteReader();
                                                if (reader6.HasRows)
                                                {
                                                    if (reader6.Read())
                                                    {
                                                        id_author = reader6.GetInt32(0);
                                                        InsertData("Series", connection, columns, $"('{series}', {id_author})");
                                                        int idLastPlusOne = GetLastId("Series", connection);
                                                        UpdateDataId(tables[tablesTemp], "Series", $"{idLastPlusOne}", idStr, id, connection);
                                                    }
                                                }
                                                else
                                                {
                                                    string columns2 = "(Author)";
                                                    InsertData("Author", connection, columns2, $"('{row.ItemArray[2]}')");
                                                    int idLastPlusOne2 = GetLastId("Author", connection);
                                                    InsertData("Series", connection, columns, $"('{series}', {idLastPlusOne2})");
                                                    int idLastPlusOne = GetLastId("Series", connection);
                                                    UpdateDataId(tables[tablesTemp], "Series", $"{idLastPlusOne}", idStr, id, connection);
                                                }
                                            }
                                            break;
                                        case 4:
                                            string genry = row.ItemArray[i].ToString();
                                            SQLiteCommand selectCommandFind3 = new SQLiteCommand($"SELECT id FROM Genre WHERE Genre='{genry}'", connection);
                                            SQLiteDataReader reader3 = selectCommandFind3.ExecuteReader();
                                            if (reader3.HasRows)
                                            {
                                                if (reader3.Read())
                                                {
                                                    UpdateDataId(tables[tablesTemp], "Genre", $"{reader3.GetInt32(0)}", idStr, id, connection);
                                                }
                                            }
                                            else
                                            {
                                                string columns = "(Genre)";
                                                InsertData("Genre", connection, columns, $"('{genry}')");
                                                int idLastPlusOne = GetLastId("Genre", connection);
                                                UpdateDataId(tables[tablesTemp], "Genre", $"{idLastPlusOne}", idStr, id, connection);
                                            }
                                            break;
                                        case 8:
                                            string reating = row.ItemArray[i].ToString();
                                            SQLiteCommand selectCommandFind7 = new SQLiteCommand($"SELECT id FROM Rating WHERE Rating='{reating}'", connection);
                                            SQLiteDataReader reader7 = selectCommandFind7.ExecuteReader();
                                            if (reader7.HasRows)
                                            {
                                                if (reader7.Read())
                                                {
                                                    UpdateDataId(tables[tablesTemp], "Rating", $"{reader7.GetInt32(0)}", idStr, id, connection);
                                                }
                                            }
                                            else
                                            {
                                                string columns = "(Rating)";
                                                InsertData("Rating", connection, columns, $"('{reating}')");
                                                int idLastPlusOne = GetLastId("Rating", connection);
                                                UpdateDataId(tables[tablesTemp], "Rating", $"{idLastPlusOne}", idStr, id, connection);
                                            }
                                            
                                            break;
                                        case 9:
                                            string status = row.ItemArray[i].ToString();
                                            SQLiteCommand selectCommandFind4 = new SQLiteCommand($"SELECT id FROM Reading_status WHERE Reading_status='{status}'", connection);
                                            SQLiteDataReader reader4 = selectCommandFind4.ExecuteReader();
                                            if (reader4.HasRows)
                                            {
                                                if (reader4.Read())
                                                {
                                                    UpdateDataId(tables[tablesTemp], "Reading_status", $"{reader4.GetInt32(0)}", idStr, id, connection);
                                                }
                                            }
                                            else
                                            {
                                                string columns = "(Reading_status)";
                                                InsertData("Reading_status", connection, columns, $"('{status}')");
                                                int idLastPlusOne = GetLastId("Reading_status", connection);
                                                UpdateDataId(tables[tablesTemp], "Reading_status", $"{idLastPlusOne}", idStr, id, connection);
                                            }
                                            break;
                                        default:
                                            UpdateData(row, i, tablesTemp, idStr, id, connection);
                                            break;
                                    }
                                }
                                // Перебор серий
                                else if (tablesTemp == 2)
                                {
                                    switch (i)
                                    {
                                        case 2:
                                            SQLiteCommand selectCommandFind2 = new SQLiteCommand($"SELECT id FROM Author WHERE Author='{row.ItemArray[i]}'", connection);
                                            SQLiteDataReader reader2 = selectCommandFind2.ExecuteReader();
                                            if (reader2.HasRows)
                                            {
                                                if (reader2.Read())
                                                {
                                                    UpdateDataId(tables[tablesTemp], "id_author", $"{reader2.GetInt32(0)}", idStr, id, connection);
                                                }
                                            }
                                            else
                                            {
                                                string columns = "(Author)";
                                                InsertData("Author", connection, columns, $"('{row.ItemArray[i]}')");
                                                int idLastPlusOne = GetLastId("Author", connection);
                                                UpdateDataId(tables[tablesTemp], "id_author", $"{idLastPlusOne}", idStr, id, connection);
                                            }
                                            break;

                                        default:
                                            UpdateData(row, i, tablesTemp, idStr, id, connection);
                                            break;
                                    }
                                }
                                // Остальные случаи
                                else
                                {
                                    UpdateData(row, i, tablesTemp, idStr, id, connection);
                                }
                            }
                        }
                    }
                    else
                    {
                        string table = tables[tablesTemp];
                        string columns;
                        switch (tablesTemp)
                        {
                            case 0:
                                if (tables.Count == 6) 
                                {
                                    columns = "(Title, Author, Series, Genre, Publishing_house, Year_of_publication, Number_of_pages, Rating, Reading_status, Start_date, Final_date)";
                                    string title = row.ItemArray[1].ToString();
                                    string author2 = row.ItemArray[2].ToString();
                                    SQLiteCommand selectCommandFind2 = new SQLiteCommand($"SELECT id FROM Author WHERE Author='{author2}'", connection);
                                    SQLiteDataReader reader2 = selectCommandFind2.ExecuteReader();
                                    if (reader2.HasRows)
                                    {
                                        if (reader2.Read())
                                        {
                                            author2 = reader2.GetValue(0).ToString();
                                        }
                                    }
                                    else
                                    {
                                        string columns2 = "(Author)";
                                        InsertData("Author", connection, columns2, $"('{author2}')");
                                        int idLast = GetLastId("Author", connection);
                                        author2 = idLast.ToString();
                                    }
                                    string Series = row.ItemArray[3].ToString();
                                    SQLiteCommand selectCommandFind5 = new SQLiteCommand($"SELECT id FROM Series WHERE Series='{Series}'", connection);
                                    SQLiteDataReader reader5 = selectCommandFind5.ExecuteReader();
                                    if (reader5.HasRows)
                                    {
                                        if (reader5.Read())
                                        {
                                            Series = reader5.GetValue(0).ToString();
                                        }
                                    }
                                    else
                                    {
                                        string columns3 = "(Series, id_author)";
                                        InsertData("Series", connection, columns3, $"('{Series}', {author2})");
                                        int idLast = GetLastId("Series", connection);
                                        Series = idLast.ToString();
                                    }
                                    string Genre = row.ItemArray[4].ToString();
                                    SQLiteCommand selectCommandFind3 = new SQLiteCommand($"SELECT id FROM Genre WHERE Genre='{Genre}'", connection);
                                    SQLiteDataReader reader4 = selectCommandFind3.ExecuteReader();
                                    if (reader4.HasRows)
                                    {
                                        if (reader4.Read())
                                        {
                                           Genre = reader4.GetValue(0).ToString();
                                        }
                                    }
                                    else
                                    {
                                        string columns4 = "(Genre)";
                                        InsertData("Genre", connection, columns4, $"('{Genre}')");
                                        int idLast = GetLastId("Genre", connection);
                                        Genre = idLast.ToString();
                                    }
                                    string Publishing_house = row.ItemArray[5].ToString();
                                    string Year_of_publication = row.ItemArray[6].ToString();
                                    string Number_of_pages = row.ItemArray[7].ToString();
                                    string reating = row.ItemArray[8].ToString();
                                    SQLiteCommand selectCommandFind7 = new SQLiteCommand($"SELECT id FROM Rating WHERE Rating='{reating}'", connection);
                                    SQLiteDataReader reader7 = selectCommandFind7.ExecuteReader();
                                    if (reader7.HasRows)
                                    {
                                        if (reader7.Read())
                                        {
                                            reating = reader7.GetValue(0).ToString();
                                        }
                                    }
                                    else
                                    {
                                        string columns7 = "(Rating)";
                                        InsertData("Rating", connection, columns7, $"('{reating}')");
                                        int idLast = GetLastId("Rating", connection);
                                        reating = idLast.ToString();
                                    }
                                    string reading_status = row.ItemArray[9].ToString();
                                    SQLiteCommand selectCommandFind4 = new SQLiteCommand($"SELECT id FROM Reading_status WHERE Reading_status='{reading_status}'", connection);
                                    SQLiteDataReader reader9 = selectCommandFind4.ExecuteReader();
                                    if (reader9.HasRows)
                                    {
                                        if (reader9.Read())
                                        {
                                            reading_status = reader9.GetValue(0).ToString();
                                        }
                                    }
                                    else
                                    {
                                        string columns9 = "(Reading_status)";
                                        InsertData("Reading_status", connection, columns9, $"('{reading_status}')");
                                        int idLast = GetLastId("Reading_status", connection);
                                        reading_status= idLast.ToString();
                                        
                                    }
                                    string Start_date = row.ItemArray[10].ToString();
                                    if (Start_date == null || Start_date == "")
                                    {
                                        Start_date = "NULL";
                                    }
                                    else
                                    {
                                        Start_date = "'" + Start_date + "'";
                                    }
                                    string Final_date = row.ItemArray[11].ToString();
                                    if (Final_date == null || Final_date == "")
                                    {
                                        Final_date = "NULL";
                                    }
                                    else
                                    {
                                        Final_date = "'" + Final_date + "'";
                                    }
                                    InsertData(table, connection, columns, $"('{title}', {author2}, {Series}, {Genre}, '{Publishing_house}', {Year_of_publication}, {Number_of_pages}, {reating}, {reading_status}, {Start_date}, {Final_date})");

                                }
                                else
                                {
                                    columns = "(Title, Author, Series, Genre, Publishing_house, Year_of_publication, Number_of_pages, Reading_status, Start_date, Final_date)";
                                    string title = row.ItemArray[1].ToString();
                                    string author2 = row.ItemArray[2].ToString();
                                    SQLiteCommand selectCommandFind2 = new SQLiteCommand($"SELECT id FROM Author WHERE Author='{author2}'", connection);
                                    SQLiteDataReader reader2 = selectCommandFind2.ExecuteReader();
                                    if (reader2.HasRows)
                                    {
                                        if (reader2.Read())
                                        {
                                            author2 = reader2.GetValue(0).ToString();
                                        }
                                    }
                                    else
                                    {
                                        string columns2 = "(Author)";
                                        InsertData("Author", connection, columns2, $"('{author2}')");
                                        int idLast = GetLastId("Author", connection);
                                        author2 = idLast.ToString();
                                    }
                                    string Series = row.ItemArray[3].ToString();
                                    SQLiteCommand selectCommandFind5 = new SQLiteCommand($"SELECT id FROM Series WHERE Series='{Series}'", connection);
                                    SQLiteDataReader reader5 = selectCommandFind5.ExecuteReader();
                                    if (reader5.HasRows)
                                    {
                                        if (reader5.Read())
                                        {
                                            Series = reader5.GetValue(0).ToString();
                                        }
                                    }
                                    else
                                    {
                                        string columns3 = "(Series, id_author)";
                                        InsertData("Series", connection, columns3, $"('{Series}', {author2})");
                                        int idLast = GetLastId("Series", connection);
                                        Series = idLast.ToString();
                                    }
                                    string Genre = row.ItemArray[4].ToString();
                                    SQLiteCommand selectCommandFind3 = new SQLiteCommand($"SELECT id FROM Genre WHERE Genre='{Genre}'", connection);
                                    SQLiteDataReader reader4 = selectCommandFind3.ExecuteReader();
                                    if (reader4.HasRows)
                                    {
                                        if (reader4.Read())
                                        {
                                            Genre = reader4.GetValue(0).ToString();
                                        }
                                    }
                                    else
                                    {
                                        string columns4 = "(Genre)";
                                        InsertData("Genre", connection, columns4, $"('{Genre}')");
                                        int idLast = GetLastId("Genre", connection);
                                        Genre = idLast.ToString();
                                    }
                                    string Publishing_house = row.ItemArray[5].ToString();
                                    string Year_of_publication = row.ItemArray[6].ToString();
                                    string Number_of_pages = row.ItemArray[7].ToString();
                                    string reading_status = row.ItemArray[8].ToString();
                                    SQLiteCommand selectCommandFind4 = new SQLiteCommand($"SELECT id FROM Reading_status WHERE Reading_status='{reading_status}'", connection);
                                    SQLiteDataReader reader9 = selectCommandFind4.ExecuteReader();
                                    if (reader9.HasRows)
                                    {
                                        if (reader9.Read())
                                        {
                                            reading_status = reader9.GetValue(0).ToString();
                                        }
                                    }
                                    else
                                    {
                                        string columns9 = "(Reading_status)";
                                        InsertData("Reading_status", connection, columns9, $"('{reading_status}')");
                                        int idLast = GetLastId("Reading_status", connection);
                                        reading_status = idLast.ToString();

                                    }
                                    string Start_date = row.ItemArray[9].ToString();
                                    if (Start_date == null || Start_date == "")
                                    {
                                        Start_date = "NULL";
                                    }
                                    else
                                    {
                                        Start_date = "'" + Start_date + "'";
                                    }
                                    string Final_date = row.ItemArray[10].ToString();
                                    if (Final_date == null || Final_date == "")
                                    {
                                        Final_date = "NULL";
                                    }
                                    else
                                    {
                                        Final_date = "'" + Final_date + "'";
                                    }
                                    InsertData(table, connection, columns, $"('{title}', {author2}, {Series}, {Genre}, '{Publishing_house}', {Year_of_publication}, {Number_of_pages}, {reading_status}, {Start_date}, {Final_date})");

                                }

                                
                                break;
                            case 2:
                                columns = $"({table}, id_author)";
                                string seria = row.ItemArray[1].ToString();
                                string author = row.ItemArray[2].ToString();
                                SQLiteCommand findAuthor = new SQLiteCommand($"SELECT id FROM Author WHERE Author='{author}'", connection);
                                SQLiteDataReader reader3 = findAuthor.ExecuteReader();
                                if (reader3.HasRows)
                                {
                                    if (reader3.Read())
                                    {
                                        InsertData(table, connection, columns, $"('{seria}', {reader3.GetInt32(0)})");
                                    }
                                }
                                else
                                {
                                    columns = "(Author)";
                                    InsertData("Author", connection, columns, $"('{author}')");
                                    int idLast = GetLastId("Author", connection);
                                    columns = $"({table}, id_author)";
                                    InsertData(table, connection, columns, $"('{seria}', {idLast})");
                                }
                                break;
                            default:
                                columns = $"({table})";
                                string value = row.ItemArray[1].ToString();
                                InsertData(table, connection, columns, $"('{value}')");
                                break;
                        }
                    }
                }
                connection.Close();
            }
        }
        private int GetLastId(string table, SQLiteConnection connection, string idStr = "id")
        {
            int id = 0;
            SQLiteCommand getId = new SQLiteCommand($"SELECT {idStr} from {table} order by id DESC limit 1;", connection);
            SQLiteDataReader reader = getId.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
            }
            return id;

        }
        private void InsertData(string table, SQLiteConnection connection, string columns, string values)
        {
            SQLiteCommand updateCommand = new SQLiteCommand($"INSERT INTO {table} {columns} VALUES {values}", connection);
            updateCommand.ExecuteNonQuery();
        }
        private void UpdateDataId(string tables, string column, string value, string idStr, int id, SQLiteConnection connection)
        {
            string command = $"UPDATE {tables} SET {column}={value} WHERE {idStr}={id}";
            SQLiteCommand updateCommand = new SQLiteCommand(command, connection);
            updateCommand.ExecuteNonQuery();
        }
        private void UpdateData(DataRow row, int i, int tablesTemp, string idStr, int id, SQLiteConnection connection)
        {
            string command = "";
            string columnName = row.Table.Columns[i].ColumnName;
            if (row.ItemArray[i] == null || row.ItemArray[i].ToString() == "")
            {
                command = $"UPDATE {tables[tablesTemp]} SET {columnName}=NULL WHERE {idStr}={id}";
                SQLiteCommand updateCommand = new SQLiteCommand(command, connection);
                updateCommand.ExecuteNonQuery();
            }
            else
            {
                string value = row.ItemArray[i].ToString().Replace("'", "");
                command = $"UPDATE {tables[tablesTemp]} SET {columnName}='{value}' WHERE {idStr}={id}";
                SQLiteCommand updateCommand = new SQLiteCommand(command, connection);
                updateCommand.ExecuteNonQuery();
            }

        }
        private int findActiveButton()
        {
            for (int i = 0; i < buttonsBD.Count; i++)
            {
                if (buttonsBD[i].Opacity == 1)
                {
                    return i;
                }
            }
            return -1;
        }
        private void Settings_Botton(object sender, RoutedEventArgs e)
        {
            DefaultSettings defaultSettings = new DefaultSettings(this);
            defaultSettings.Show();
            this.Hide();

        }

        private void CatTeam_Botton(object sender, RoutedEventArgs e)
        {
            CatCommand catCommand = new CatCommand(this);
            catCommand.Show();
            this.Hide();
        }

        private void Team_Button(object sender, RoutedEventArgs e)
        {
            TeamLid teamLid = new TeamLid(this);
            teamLid.Show();
            this.Hide();
        }

        private void ListBooks_Button(object sender, RoutedEventArgs e)
        {
            ClearButton();
            buttonsBD[0].Opacity = 1;
            WriteDataGrid(sqlquere);
        }

        private void Author_Button(object sender, RoutedEventArgs e)
        {
            ClearButton();
            buttonsBD[1].Opacity = 1;
            WriteDataGrid(sqlAnother + "Author");
        }

        private void Series_Button(object sender, RoutedEventArgs e)
        {
            ClearButton();
            buttonsBD[2].Opacity = 1;
            WriteDataGrid(sqlSeries);
        }

        private void Genre_Button(object sender, RoutedEventArgs e)
        {
            ClearButton();
            buttonsBD[3].Opacity = 1;
            WriteDataGrid(sqlAnother + "Genre");
        }

        private void ReadingStatus_Button(object sender, RoutedEventArgs e)
        {
            ClearButton();
            buttonsBD[4].Opacity = 1;
            WriteDataGrid(sqlAnother + "Reading_status");
        }

        private void Rating_Click(object sender, RoutedEventArgs e)
        {
            if (buttonsBD.Count == 6)
            {
                ClearButton();
                buttonsBD[5].Opacity = 1;
                WriteDataGrid(sqlAnother + "Rating");
            }
        }

        private void MainGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Search_Button(object sender, RoutedEventArgs e)
        {
            Search search = new Search(tables, NameDB);
            search.ShowDialog();
        }
    }
}
