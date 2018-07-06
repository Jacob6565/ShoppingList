using BooksMVVM.Model;
using BooksMVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BooksMVVM.View
{
    /// <summary>
    /// Describes the page for adding a product together with the corresponding .xaml file.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddBookPage : ContentPage
    {
        /// <summary>
        /// Stores the corresponding viewmodel
        /// </summary>
        private AddBookPageViewModel viewModel;
        /// <summary>
        /// Creates a new instance of the AddBookPage class.
        /// </summary>
        public AddBookPage(AddBookPageViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.BindingContext = viewModel;
            //This view then listens to a message sent by AddBookPageViewModel with the name "DisplayAlert"
            //And then perform the following action.
            MakeMessagingCenterSubscribtions();            
        }
        /// <summary>
        /// Creates the needed subscribtions regarding the messagingcenter.
        /// </summary>
        private void MakeMessagingCenterSubscribtions()
        {
            //Makes a subscribtions for message send by the type "AddBookPageViewModel", which got 
            //parameters, in this case an array of strings. It listens to the message with name "ProductNotDeleted"
            //and performs the action described by the lambda expression.
            //Does not violate MVVM, since the ViewModel still knows nothing about the View.
            MessagingCenter.Subscribe<AddBookPageViewModel, string[]>(this, "ProductNotDeleted", (senderViewModel, args) =>
            {
                DisplayAlert(args[0], args[1], args[2]);
            });

            MessagingCenter.Subscribe<AddBookPageViewModel, string[]>(this, "ProductAlreadyAdded", (senderViewModel, args) =>
            {
                DisplayAlert(args[0], args[1], args[2]);
            });

            MessagingCenter.Subscribe<AddBookPageViewModel, string[]>(this, "ProductAdded", (senderViewModel, args) =>
            {
                DisplayAlert(args[0], args[1], args[2]);
            });

            MessagingCenter.Subscribe<AddBookPageViewModel, string[]>(this, "ProductNotAdded", (senderViewModel, args) =>
            {
                DisplayAlert(args[0], args[1], args[2]);
            });

            MessagingCenter.Subscribe<AddBookPageViewModel, string[]>(this, "ProductDeleted", (senderViewModel, args) =>
            {
                DisplayAlert(args[0], args[1], args[2]);
            });
        }

        /// <summary>
        /// Defines what happens when the pase appears
        /// </summary>
        protected override void OnAppearing()
        {
            //Does not violate MVVM, since the ViewModel still knows nothing about the View.
            base.OnAppearing();
            viewModel.UpdateLocalBooks();
        }

    }
}