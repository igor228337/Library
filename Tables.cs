using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class ListOfBooks
    {
        public ListOfBooks(long Id, string Title, string Author, string Series, string Genre, string Publishing_house, string Year_of_publication, long Number_of_pages, string Rating, string Reading_status, string Start_date, string Final_date)
        {
            this.Id = Id;
            this.Title = Title;
            this.Author = Author;
            this.Series = Series;
            this.Genre = Genre;
            this.Publishing_house = Publishing_house;
            this.Year_of_publication = Year_of_publication;
            this.Number_of_pages = Number_of_pages;
            this.Rating = Rating;
            this.Reading_status = Reading_status;
            this.Start_date = Start_date;
            this.Final_date = Final_date;
        }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Series { get; set; }
        public string Genre { get; set; }
        public string Publishing_house { get; set; }
        public string Year_of_publication { get; set; }
        public long Number_of_pages { get; set; }
        public string Rating { get; set; }
        public string Reading_status { get; set; }
        public string Start_date { get; set; }
        public string Final_date { get; set; }
    }
    class ListOfBooksP
    {
        public ListOfBooksP(long Id, string Title, string Author, string Series, string Genre, string Publishing_house, string Year_of_publication, long Number_of_pages, string Reading_status, string Start_date, string Final_date)
        {
            this.Id = Id;
            this.Title = Title;
            this.Author = Author;
            this.Series = Series;
            this.Genre = Genre;
            this.Publishing_house = Publishing_house;
            this.Year_of_publication = Year_of_publication;
            this.Number_of_pages = Number_of_pages;
            this.Reading_status = Reading_status;
            this.Start_date = Start_date;
            this.Final_date = Final_date;
        }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Series { get; set; }
        public string Genre { get; set; }
        public string Publishing_house { get; set; }
        public string Year_of_publication { get; set; }
        public long Number_of_pages { get; set; }
        public string Reading_status { get; set; }
        public string Start_date { get; set; }
        public string Final_date { get; set; }
    }
    class Author
    {
        public Author(long Id, string Author)
        {
            this.Id = Id;
            this.AuthorName = Author;
        }
        public long Id { get; set; }
        public string AuthorName { get; set; }
    }
    class Series
    {
        public Series(long Id, string Series)
        {
            this.Id = Id;
            this.SeriesName = Series;
        }
        public long Id { get; set; }
        public string SeriesName { get; set; }
    }
    class Genre
    {
        public Genre(long Id, string Genre)
        {
            this.Id = Id;
            this.GenreName = Genre;
        }
        public long Id { get; set; }
        public string GenreName { get; set; }
    }
    class Rating
    {
        public Rating(long Id, string Rating)
        {
            this.Id = Id;
            this.RatingName = Rating;
        }
        public long Id { get; set; }
        public string RatingName { get; set; }
    }
    class ReadingStatus
    {
        public ReadingStatus(long Id, string Reading_status)
        {
            this.Id = Id;
            this.ReadingStatusName = Reading_status;
        }
        public long Id { get; set; }
        public string ReadingStatusName { get; set; }
    }
}
