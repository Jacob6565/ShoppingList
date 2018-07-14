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
        public ObservableCollection<Product> RetrieveProductsFromDatabase(Comparison<Product> comparison)
        {
            ObservableCollection<Product> returnValue;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Product>();
                var temp = conn.Table<Product>().ToList();
                temp.Sort(comparison);
                returnValue = new ObservableCollection<Product>(temp);

            }
            return returnValue;
        }

        /// <summary>
        /// Retrieves the products from the database which meets the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public ObservableCollection<Product> RetrieveBooksFromDatabase(Func<Product, bool> predicate)
        {
            ObservableCollection<Product> returnValue;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Product>();
                returnValue = new ObservableCollection<Product>(conn.Table<Product>().Where(predicate));

            }
            return returnValue;
        }

        /// <summary>
        /// Retrieves the products from the database.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Product> RetrieveProductsFromDatabase()
        {
            ObservableCollection<Product> returnValue;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Product>();
                returnValue = new ObservableCollection<Product>(conn.Table<Product>().ToList());

            }
            return returnValue;
        }

        /// <summary>
        /// Updates the corresponding book in the database, however it does not check if it already exists.
        /// </summary>
        /// <param name="product"></param>
        public void UpdateProductInDatabase(Product product)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
               conn.Update(product);
            }
        }

        /// <summary>
        /// Updates the corresponding books in the database, however it does not check if they already exists.
        /// </summary>
        /// <param name="product"></param>
        public void UpdateProductsInDatabase(List<Product> product)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.UpdateAll(product);
            }
        }

        /// <summary>
        /// Adds the corresponding book to the database, however it does not check if the table is created.
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public bool AddProductToDatabase(Product product)
        {
            int resultFromInsertion = 0;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                resultFromInsertion = conn.Insert(product);    
            }
            return resultFromInsertion > 0;
        }
        
        /// <summary>
        /// Deletes the corresponding product in the database, however it does not check if it exists.
        /// </summary>
        /// <param name="products"></param>
        public void DeleteProductsFromDatabase(List<Product> products)
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
               foreach(Product product in products)
                {
                    conn.Delete(product);
                }
            }
        }

    }
}
