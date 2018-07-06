using System;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;

using BooksMVVM.Model;

using Xamarin.Forms;

namespace BooksMVVM.ViewModel
{
    /// <summary>
    /// ViewModel for the AddBookPage
    /// </summary>
    public class AddBookPageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        #region IO-Messages
        private string Ok { get => "Ok!"; }
        private string FailedToAdd { get => "Failed to add your product!"; }
        private string Failure { get => "Failed!"; }
        private string Success { get => "Success!"; }
        private string SuccessedToAdd { get => "Your product has been sucessfully added!"; }
        private string Great { get => "Great!"; }
        private string Oops { get => "Oops!"; }
        private string AlreadyAdded { get => "This product has already been added!"; }
        private string Retry { get => "Retry!"; }
        private string DoNotExist { get => "The product you tried to remove does not exist!"; }
        private string Deleted { get => "Your product has been sucessfully removed"; }
        #endregion

        /// <summary>
        /// Initialized a new instance of the AddBookPageViewModel class.
        /// </summary>
        public AddBookPageViewModel()
        {
            //Creates the commands bound to from the view.
            AddBookCommand = new Command(AddBookCommand_Execute, CanExecute);
            DeleteBookCommand = new Command(DeleteBookCommand_Execute, CanExecute);
        }

        private string _nameOfBookToAdd;
        private string _shopOfBookToAdd;
        private string _priceOfBookToAdd;
        
        /// <summary>
        /// Gets or sets the name of the product which is about to be added.
        /// </summary>
        /// This is bound to an entry in the view. 
        /// This allows for the viewmodel to access value of xaml element.
        public string NameOfBookToAdd { get => _nameOfBookToAdd; set
            {
                _nameOfBookToAdd = value;
                //Notify dependent entities that this property has changed.
                NotifyPropertyChanged();
                //Indication that this value has been changed and that CanExecute should be reevaluated.
                //Just like we call NotifyPropertyChanged to indicate that the property has changed.
                ((Command)AddBookCommand).ChangeCanExecute();
                ((Command)DeleteBookCommand).ChangeCanExecute();

            }
        }
        /// <summary>
        /// Gets or sets the shop of the product which is about to be added.
        /// </summary>
        public string ShopOfBookToAdd { get => _shopOfBookToAdd; set
            {
                _shopOfBookToAdd = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the name of the product which is about to be added.
        /// </summary>
        public string PriceOfBookToAdd { get => _priceOfBookToAdd; set
            {
                _priceOfBookToAdd = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Command for adding the book to the database.
        /// </summary>
        public ICommand AddBookCommand { get; private set; }

        /// <summary>
        /// Command for deleting the book from the database.
        /// </summary>
        public ICommand DeleteBookCommand { get; private set; }

        /// <summary>
        /// Deletes the book from the database
        /// </summary>
        public void DeleteBookCommand_Execute()
        {
            //Creates the book to be removed.
            Book bookToRemove = new Book()
            {
                Name = NameOfBookToAdd,
                Shop = ShopOfBookToAdd,
                Price = Convert.ToDouble(PriceOfBookToAdd),
                IsVisible = false
            };

            if (Books.Contains(bookToRemove))
            {
                //For some reason I can't use .Equal, it does not work..
                List<Book> booksToBeRemoved = Books.ToList().FindAll(book => book.Name.Equals(bookToRemove.Name));
                DatabaseHelper.DeleteBooksFromDatabase(booksToBeRemoved);
                SendMessagingCenterMessage("ProductDeleted", Success, Deleted, Great);
                ClearEntries();
            }
            else
            {                
                SendMessagingCenterMessage("ProductNotDeleted", Failure, DoNotExist, Ok);
            }
        }    
        
       
        /// <summary>
        /// Returns a boolean indicating wheter the product
        /// expressed by the entries can be added or deleted.
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            return ErrorCheckingAddingProduct();
        }

        /// <summary>
        /// Contains the logic for adding a product.
        /// </summary>
        public void AddBookCommand_Execute()
        {
            Book bookToAdd = new Book()
            {
                Name = NameOfBookToAdd,
                Shop = ShopOfBookToAdd,
                Price = Convert.ToDouble(PriceOfBookToAdd),
                IsVisible = false
            };

            if (Books.Contains(bookToAdd))
            {
               SendMessagingCenterMessage("ProductAlreadyAdded", Oops, AlreadyAdded, Ok);
            }
            else
            {
                bool success = DatabaseHelper.AddBookToDatabase(bookToAdd);

                if (success)
                {                    
                    SendMessagingCenterMessage("ProductAdded", Success, SuccessedToAdd, Great);
                    ClearEntries();
                }
                else
                {
                    SendMessagingCenterMessage("ProductNotAdded", Failure, FailedToAdd, Retry);
                }
            }
        }


        /// <summary>
        /// Returns a boolean indicating whether the input is valid before adding or deleting.
        /// </summary>
        /// <returns></returns>
        private bool ErrorCheckingAddingProduct()
        {
            if (!String.IsNullOrWhiteSpace(NameOfBookToAdd))
            {
                bool nameEntryResult = NameOfBookToAdd.All(c => Char.IsLetterOrDigit(c) || c == '-' || c == '(' || c == ')');

                return nameEntryResult;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Clears the entries for text after adding/deleting a product.
        /// </summary>
        private void ClearEntries()
        {
            NameOfBookToAdd = String.Empty;
            ShopOfBookToAdd = String.Empty;
            PriceOfBookToAdd = String.Empty;
        }

        /// <summary>
        /// Used to update the local representation of a table in the database.
        /// </summary>
        public void UpdateLocalBooks()
        {
            Books = DatabaseHelper.RetrieveBooksFromDatabase();
        }

        /// <summary>
        /// Auxiliary function for sending message through the MessagingCenter.
        /// </summary>
        /// <param name="nameOfMessage"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="confirm"></param>
        private void SendMessagingCenterMessage(string nameOfMessage, string title, string message, string confirm)
        {
            string[] args = new string[]
                    {
                        title,
                        message,
                        confirm
                    };

            MessagingCenter.Send(this, nameOfMessage, args);
        }
    }
}
