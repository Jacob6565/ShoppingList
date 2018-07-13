using SQLite;
using System;


namespace BooksMVVM.Model
{
    /// <summary>
    /// Class representing a product.
    /// </summary>
    public class Book : IProduct //No need for INotifyPropertyChanged since ObservableCollection is used.
    {
        /// <summary>
        /// Used by the database for assigning primary keys to it's elements.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets string containing the name and price of product.
        /// </summary>
        private string nameAndPrice = String.Empty;

        /// <summary>
        /// Gets a string containing the name and price of the product.
        /// </summary>
        public string NameAndPrice
        {
            get
            {
                return Name + " - " + Price + " kr";
            }
            set { nameAndPrice = value; }
        }

        /// <summary>
        /// Gets the shop where the product can be bought.
        /// </summary>
        public string Shop { get; set; }

        /// <summary>
        /// Gets the price of the product.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Indicates whether or not the current product is on the shoppinglist.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Used to compare two products.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int IComparable.CompareTo(Object obj)
        {
            Book compareObj = obj as Book;
            if (this.Name.Equals(compareObj.Name) && this.Shop.Equals(compareObj.Shop))
                return 0;
            else
                return 1;
        }

        /// <summary>
        /// Used to determine if two products are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>s
        bool IEquatable<IProduct>.Equals(IProduct other)
        {
            return this.Name.Equals(other.Name) && this.Shop.Equals(other.Shop);
        }
    }
}
