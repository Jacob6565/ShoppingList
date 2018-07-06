using System;
using System.Linq;
using System.Windows.Input;
using BooksMVVM.Model;
using Xamarin.Forms;

namespace BooksMVVM.ViewModel
{
    /// <summary>
    /// The ViewModel used for the MakeListPage
    /// </summary>
    public class MakeListPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Used to determines how to sort the ListView. Standard is by shop.
        /// </summary>
        Comparison<Book> comparison = new Comparison<Book>((bookX, bookY) => bookX.Shop.CompareTo(bookY.Shop));

        /// <summary>
        /// Initializes a new instance of the MakeListPageViewModel class.
        /// </summary>
        public MakeListPageViewModel()
        {
            //Creates the commands bound to from the view.           
            SortByName_Command = new Command(SortByName_Command_Execute, CanListBeSorted);
            SortByShop_Command = new Command(SortByShop_Command_Execute, CanListBeSorted);
        }

        /// <summary>
        /// Used to update the local representation of the database.
        /// </summary>
        public void UpdateLocalBooks()
        {
            Books = DatabaseHelper.RetrieveBooksFromDatabase(comparison);

            //Reevaluate if the commands can be executed.
            ((Command)SortByShop_Command).ChangeCanExecute();
            ((Command)SortByName_Command).ChangeCanExecute();
        }

        private Book _selectedItem;

        /// <summary>
        /// Gets or sets the currently selected item in the ListView.
        /// </summary>
        public Book SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
                InvertVisibilityOfBook(value);
            }
        }

        /// <summary>
        /// Inverts the visibility of the currently selected book and updates db.
        /// </summary>
        /// <param name="selectedBook"></param>
        public void InvertVisibilityOfBook(Book selectedBook)
        {
            Book bookToChange = Books.ToList().Find(book => selectedBook.ID == book.ID);
            bookToChange.IsVisible = !bookToChange.IsVisible;
            DatabaseHelper.UpdateBookInDatabase(bookToChange);
            Books = DatabaseHelper.RetrieveBooksFromDatabase(comparison);
        }

        /// <summary>
        /// Command for sorting the listView by Name.
        /// </summary>
        public ICommand SortByName_Command { get; private set; }

        /// <summary>
        /// Command for sorting the listView by Shop.
        /// </summary>
        public ICommand SortByShop_Command { get; private set; }

        /// <summary>
        /// Defines the behaviour for SortByName Command.
        /// </summary>
        public void SortByName_Command_Execute()
        {
            comparison = new Comparison<Book>((bookX, bookY) => bookX.Name.CompareTo(bookY.Name));
            Books = DatabaseHelper.RetrieveBooksFromDatabase(comparison);
        }


        /// <summary>
        /// Defines the behaviour for SortByShop Command.
        /// </summary>
        public void SortByShop_Command_Execute()
        {
            comparison = new Comparison<Book>((bookX, bookY) => bookX.Shop.CompareTo(bookY.Shop));
            Books = DatabaseHelper.RetrieveBooksFromDatabase(comparison);
        }

        /// <summary>
        /// Return a boolean indicating whether the listview can be sorted.
        /// </summary>
        /// <returns></returns>
        public bool CanListBeSorted()
        {
            return Books.Count == 0 ? false : true;
        }

    }
}
