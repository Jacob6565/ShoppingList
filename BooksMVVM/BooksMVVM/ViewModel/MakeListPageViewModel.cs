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
    public class MakeListPageViewModel : BaseViewModel, IMakeListPageViewModel
    {
        /// <summary>
        /// Used to determines how to sort the ListView. Standard is by shop.
        /// </summary>
        private Comparison<Book> comparison = new Comparison<Book>((bookX, bookY) => bookX.Shop.CompareTo(bookY.Shop));

        /// <summary>
        /// Used to access the database.
        /// </summary>
        MainDAL DAL;
        /// <summary>
        /// Initializes a new instance of the MakeListPageViewModel class.
        /// </summary>
        public MakeListPageViewModel(MainDAL dal)
        {
            //Creates the commands bound to from the view.           
            SortByName_Command = new Command(SortByName_Command_Execute, CanListBeSorted);
            SortByShop_Command = new Command(SortByShop_Command_Execute, CanListBeSorted);
            DAL = dal;
        }

        /// <summary>
        /// Used to update the local representation of the database.
        /// </summary>
        public void UpdateLocalBooks()
        {
            Books = DAL.RetrieveBooksFromDatabase(comparison);

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
        private void InvertVisibilityOfBook(Book selectedBook)
        {
            Book bookToChange = Books.ToList().Find(book => selectedBook.ID == book.ID);
            bookToChange.IsVisible = !bookToChange.IsVisible;
            DAL.UpdateBookInDatabase(bookToChange);
            Books = DAL.RetrieveBooksFromDatabase(comparison);
        }

        /// <summary>
        /// Command for sorting the listView by Name.
        /// </summary>
        public ICommand SortByName_Command { get; set; }

        /// <summary>
        /// Command for sorting the listView by Shop.
        /// </summary>
        public ICommand SortByShop_Command { get; set; }

        /// <summary>
        /// Defines the behaviour for SortByName Command.
        /// </summary>
        private void SortByName_Command_Execute()
        {
            comparison = new Comparison<Book>((bookX, bookY) => bookX.Name.CompareTo(bookY.Name));
            Books = DAL.RetrieveBooksFromDatabase(comparison);
        }


        /// <summary>
        /// Defines the behaviour for SortByShop Command.
        /// </summary>
        private void SortByShop_Command_Execute()
        {
            comparison = new Comparison<Book>((bookX, bookY) => bookX.Shop.CompareTo(bookY.Shop));
            Books = DAL.RetrieveBooksFromDatabase(comparison);
        }

        /// <summary>
        /// Return a boolean indicating whether the listview can be sorted.
        /// </summary>
        /// <returns></returns>
        private bool CanListBeSorted()
        {
            return Books.Count == 0 ? false : true;
        }

    }
}
