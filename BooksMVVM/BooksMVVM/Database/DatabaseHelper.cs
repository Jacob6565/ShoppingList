using BooksMVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BooksMVVM
{
    /// <summary>
    /// Contains helping methods for interacting with the database.
    /// </summary>
    public class MainDAL
    {
        /// <summary>
        /// Retrieves the products from the database sorted according to the parameter.
        /// </summary>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public ObservableCollection<Book> RetrieveBooksFromDatabase(Comparison<Book> comparison)
        {
            ObservableCollection<Book> returnValue;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Book>();
                var temp = conn.Table<Book>().ToList();
                temp.Sort(comparison);
                returnValue = new ObservableCollection<Book>(temp);

            }
            return returnValue;
        }

        /// <summary>
        /// Retrieves the products from the database which meets the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public ObservableCollection<Book> RetrieveBooksFromDatabase(Func<Book, bool> predicate)
        {
            ObservableCollection<Book> returnValue;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Book>();
                returnValue = new ObservableCollection<Book>(conn.Table<Book>().Where(predicate));

            }
            return returnValue;
        }

        /// <summary>
        /// Retrieves the products from the database.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Book> RetrieveBooksFromDatabase()
        {
            ObservableCollection<Book> returnValue;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Book>();
                returnValue = new ObservableCollection<Book>(conn.Table<Book>().ToList());

            }
            return returnValue;
        }

        /// <summary>
        /// Updates the corresponding book in the database, however it does not check if it already exists.
        /// </summary>
        /// <param name="book"></param>
        public void UpdateBookInDatabase(Book book)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
               conn.Update(book);
            }
        }

        /// <summary>
        /// Updates the corresponding books in the database, however it does not check if they already exists.
        /// </summary>
        /// <param name="book"></param>
        public void UpdateBooksInDatabase(List<Book> book)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.UpdateAll(book);
            }
        }

        /// <summary>
        /// Adds the corresponding book to the database, however it does not check if the table is created.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public bool AddBookToDatabase(Book book)
        {
            int resultFromInsertion = 0;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                resultFromInsertion = conn.Insert(book);    
            }
            return resultFromInsertion > 0;
        }
        
        /// <summary>
        /// Deletes the corresponding product in the database, however it does not check if it exists.
        /// </summary>
        /// <param name="books"></param>
        public void DeleteBooksFromDatabase(List<Book> books)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
               foreach(Book book in books)
                {
                    conn.Delete(book);
                }
            }
        }

    }
}
