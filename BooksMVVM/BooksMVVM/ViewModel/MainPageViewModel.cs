using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;

using BooksMVVM.Model;
using BooksMVVM.View;

using Xamarin.Forms;

namespace BooksMVVM.ViewModel
{
    /// <summary>
    /// ViewModel used for the MainPage
    /// </summary>
    public class MainPageViewModel : BaseViewModel, IMainPageViewModel
    {
        /// <summary>
        /// Delegate used to retrieve visible books.
        /// </summary>
        private Func<Product, bool> GetVisibleProducts = new Func<Product, bool>(product => product.IsVisible == true);

        //The two other pages, used to navigate.
        private AddProductPage addProductPage;
        private MakeListPage makeListPage;

        /// <summary>
        /// Used to access the database.
        /// </summary>
        MainDAL DAL;
        /// <summary>
        /// Initializes a new instance of the MainPageViewModel class.
        /// </summary>
        /// <param name="addBookPage"></param>
        /// <param name="makeListPage"></param>
        public MainPageViewModel(AddProductPage addProductPage, MakeListPage makeListPage, MainDAL dal)
        {
            this.addProductPage = addProductPage;
            this.makeListPage = makeListPage;
            DAL = dal;

            //Creates the commands bound to from the view.
            ToolbarItem_ADD_Command = new Command(ToolbarItem_ADD_Command_Execute);
            ToolbarItem_FILL_Command = new Command(ToolbarItem_FILL_Command_Execute);
            ClearBtn_Command = new Command(ClearBtn_Command_Execute, ClearBtn_Command_CanExecute);
            DeleteModeBtn_Command = new Command(DeleteModeBtn_Command_Execute, DeleteModeBtn_Command_CanExecute);
        }

        /// <summary>
        /// Used to update the data to be represented in the listview.
        /// </summary>
        public void UpdateLocalProducts()
        {
            Products = DAL.RetrieveBooksFromDatabase(GetVisibleProducts);
            ((Command)DeleteModeBtn_Command).ChangeCanExecute();
            ((Command)ClearBtn_Command).ChangeCanExecute();
            TotalAmount = CalculateTotalAmount();
        }

       
        private double _totalAmount;

        /// <summary>
        /// Gets or sets the total cost of the shoppinglist.
        /// </summary>
        public double TotalAmount
        {
            get
            {
                return _totalAmount;
            }
            set
            {
                _totalAmount = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Used to perform navigation between pages.
        /// </summary>
        public INavigation Navigation { get; set; }


        /// <summary>
        /// Command for the "ADD" toolbaritem.
        /// </summary>
        public ICommand ToolbarItem_ADD_Command { get; set; }

        /// <summary>
        /// Command for the "FILL" toolbaritem.
        /// </summary>
        public ICommand ToolbarItem_FILL_Command { get; set; }

        /// <summary>
        /// Command for the Clear button.
        /// </summary>
        public ICommand ClearBtn_Command { get; set; }

        /// <summary>
        /// Command for the Delete mode button.
        /// </summary>
        public ICommand DeleteModeBtn_Command { get; set; }


        /// <summary>
        /// Defines the behaviour of the DeleteMode button.
        /// </summary>
        private void DeleteModeBtn_Command_Execute()
        {
            IsDeleteMode = !IsDeleteMode;            
        }

        /// <summary>
        /// Return a boolean indicate if the DeleteMode button can be used.
        /// </summary>
        /// <returns></returns>
        private bool DeleteModeBtn_Command_CanExecute()
        {
            bool returnvalue = Products.Count == 0 ? false : true;
            if (!returnvalue) IsDeleteMode = false;
            return returnvalue;
        }

        /// <summary>
        /// Defines the behaviour of the Clear button.
        /// </summary>
        private void ClearBtn_Command_Execute()
        {
            List<Product> changedBooks = Products.ToList();
            changedBooks.ForEach(book => book.IsVisible = false);
            DAL.UpdateProductsInDatabase(changedBooks);
            Products = DAL.RetrieveBooksFromDatabase(GetVisibleProducts);
            TotalAmount = 0;
            ((Command)DeleteModeBtn_Command).ChangeCanExecute();
            ((Command)ClearBtn_Command).ChangeCanExecute();
        }

        /// <summary>
        /// Return a boolean indicate if the DeleteMode button can be used.
        /// </summary>
        /// <returns></returns>
        private bool ClearBtn_Command_CanExecute()
        {
            return Products.Count == 0 ? false : true;
        }

        /// <summary>
        /// Defines the behaviour of the toolbar item "ADD".
        /// </summary>
        private void ToolbarItem_ADD_Command_Execute()
        {
            Navigation.PushAsync(addProductPage);
        }

        /// <summary>
        /// Defines the behaviour of the toolbar item "FILL".
        /// </summary>
        private void ToolbarItem_FILL_Command_Execute()
        {
            Navigation.PushAsync(makeListPage);
        }

        private Product _selectedItem;

        /// <summary>
        /// Gets or sets the currently selected item in the ListView.
        /// </summary>
        public Product SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
                //If delete mode is enabled, do the following:
                if (IsDeleteMode)
                    IfDeleteMode(value);

                //Reevaluate if the buttons can be used.
                ((Command)DeleteModeBtn_Command).ChangeCanExecute();
                ((Command)ClearBtn_Command).ChangeCanExecute();
            }
        }

        /// <summary>
        /// Gets or sets whether delete mode is enabled.
        /// </summary>
        public bool IsDeleteMode { get; set; } = false;

        /// <summary>
        /// Defines the behaviour of selecting an item in the listview if delete mode is enabled.
        /// </summary>
        /// <param name="SelectedItem"></param>
        private void IfDeleteMode(Product SelectedItem)
        {
            //Finds the selected product.
            Product bookToChange = Products.ToList().Find(book => SelectedItem.ID == book.ID);
            bookToChange.IsVisible = false;
            //Pushes updates to the database.
            DAL.UpdateProductInDatabase(bookToChange);
            //Then updating the local representation aswell.
            Products = DAL.RetrieveBooksFromDatabase(GetVisibleProducts);
            TotalAmount -= SelectedItem.Price;
        }

        /// <summary>
        /// Calculates the total cost of the products in the shoppinglist.
        /// </summary>
        /// <returns></returns>
        private double CalculateTotalAmount()
        {
            double returnValue = 0;
            foreach (Product book in Products)
            {
                returnValue += book.Price;
            }
            return returnValue;
        }
    }
}
