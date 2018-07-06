using BooksMVVM.ViewModel;
using Xamarin.Forms;

namespace BooksMVVM.View
{
    /// <summary>
    /// Represents the MainPage together with the corresponding .xaml file.
    /// </summary>
	public partial class MainPage : ContentPage
	{
        private MainPageViewModel viewModel;
        /// <summary>
        /// Creates an instance of the MainPage class.
        /// </summary>
        public MainPage(MainPageViewModel viewModel)
		{
            InitializeComponent();
            //Assign the navigation, so the viewmodel can navigate between pages.
            viewModel.Navigation = this.Navigation;
            this.viewModel = viewModel;
            this.BindingContext = viewModel;
		}

        /// <summary>
        /// Defines what happens when the page appears
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Updates the local representation of the products,
            //since they can have been changed by other pages.
            viewModel.UpdateLocalBooks();
        }
    }
}
