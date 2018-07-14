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
    public class AddProductPageViewModel : BaseViewModel, IAddProductPageViewModel 
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

        MainDAL DAL; 
        /// <summary>
        /// Initialized a new instance of the AddBookPageViewModel class.
        /// </summary>
        public AddProductPageViewModel(MainDAL dal)
        {
            //Creates the commands bound to from the view.
            AddProductCommand = new Command(AddProductCommand_Execute, CanExecute);
            DeleteProductCommand = new Command(DeleteProductCommand_Execute, CanExecute);
            DAL = dal;
        }

        private string _nameOfProductToAdd;
        private string _shopOfProductToAdd;
        private string _priceOfProductToAdd;
        
        /// <summary>
        /// Gets or sets the name of the product which is about to be added.
        /// </summary>
        /// This is bound to an entry in the view. 
        /// This allows for the viewmodel to access value of xaml element.
        public string NameOfProductToAdd { get => _nameOfProductToAdd; set
            {
                _nameOfProductToAdd = value;
                //Notify dependent entities that this property has changed.
                NotifyPropertyChanged();
                //Indication that this value has been changed and that CanExecute should be reevaluated.
                //Just like we call NotifyPropertyChanged to indicate that the property has changed.
                ((Command)AddProductCommand).ChangeCanExecute();
                ((Command)DeleteProductCommand).ChangeCanExecute();

            }
        }
        /// <summary>
        /// Gets or sets the shop of the product which is about to be added.
        /// </summary>
        public string ShopOfProductToAdd { get => _shopOfProductToAdd; set
            {
                _shopOfProductToAdd = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the name of the product which is about to be added.
        /// </summary>
        public string PriceOfProductToAdd { get => _priceOfProductToAdd; set
            {
                _priceOfProductToAdd = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Command for adding the book to the database.
        /// </summary>
        public ICommand AddProductCommand { get; set; }

        /// <summary>
        /// Command for deleting the book from the database.
        /// </summary>
        public ICommand DeleteProductCommand { get; set; }

        /// <summary>
        /// Deletes the book from the database
        /// </summary>
        private void DeleteProductCommand_Execute()
        {
            //Creates the book to be removed.
            Product productToRemove = new Product()
            {
                Name = NameOfProductToAdd,
                Shop = ShopOfProductToAdd,
                Price = Convert.ToDouble(PriceOfProductToAdd),
                IsVisible = false
            };

            if (Products.Contains(productToRemove))
            {
                //For some reason I can't use .Equal, it does not work..
                List<Product> productsToBeRemoved = Products.ToList().FindAll(product => product.Name.Equals(productToRemove.Name));
                DAL.DeleteProductsFromDatabase(productsToBeRemoved);
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
        private bool CanExecute()
        {
            return ErrorCheckingAddingProduct();
        }

        /// <summary>
        /// Contains the logic for adding a product.
        /// </summary>
        private void AddProductCommand_Execute()
        {
            Product productToAdd = new Product()
            {
                Name = NameOfProductToAdd,
                Shop = ShopOfProductToAdd,
                Price = Convert.ToDouble(PriceOfProductToAdd),
                IsVisible = false
            };

            if (Products.Contains(productToAdd))
            {
               SendMessagingCenterMessage("ProductAlreadyAdded", Oops, AlreadyAdded, Ok);
            }
            else
            {
                bool success = DAL.AddProductToDatabase(productToAdd);

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
            if (!String.IsNullOrWhiteSpace(NameOfProductToAdd))
            {
                bool nameEntryResult = NameOfProductToAdd.All(c => Char.IsLetterOrDigit(c) || c == '-' || c == '(' || c == ')');

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
            NameOfProductToAdd = String.Empty;
            ShopOfProductToAdd = String.Empty;
            PriceOfProductToAdd = String.Empty;
        }

        /// <summary>
        /// Used to update the local representation of a table in the database.
        /// </summary>
        public void UpdateLocalProducts()
        {
            Products = DAL.RetrieveProductsFromDatabase();
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
