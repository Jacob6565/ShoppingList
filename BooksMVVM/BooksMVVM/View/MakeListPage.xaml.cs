using BooksMVVM.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BooksMVVM.View
{
    /// <summary>
    /// Represents the page for filling the shopping list together with the corresponding .xaml file.
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MakeListPage : ContentPage
	{
        /// <summary>
        /// Used to store the corresponding viewmodel.
        /// </summary>
        private IMakeListPageViewModel viewModel;
        /// <summary>
        /// Creates an instance of the MakeListPage class.
        /// </summary>
		public MakeListPage (IMakeListPageViewModel viewModel)
		{
            InitializeComponent ();
            this.viewModel = viewModel;
            this.BindingContext = viewModel;
        }
        /// <summary>
        /// Defines the behaviour of when the page appears.
        /// </summary>
        protected override void OnAppearing()
        {
            //Does not violate MVVM since ViewModel still does not know about the view.
            base.OnAppearing();
            viewModel.UpdateLocalBooks();                   
        }



    }
}