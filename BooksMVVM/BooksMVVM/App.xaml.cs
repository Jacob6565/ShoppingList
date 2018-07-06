using BooksMVVM.View;
using BooksMVVM.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace BooksMVVM
{
	public partial class App : Application
	{
        /// <summary>
        /// Path to the local database accessed via SQLite
        /// </summary>
        public static string DB_PATH = String.Empty;
        //Former constructor.
		/*public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
		}*/

        /// <summary>
        /// Initializes a new instance of the App Class
        /// </summary>
        /// <param name="path"></param>
        public App(string path)
        {            
            InitializeComponent();

            //Gets the database path send from BooksMVVM.Android.MainActivity.cs
            DB_PATH = path;

            //Instansiates the different viewmodels and pages.
            AddBookPageViewModel addBookPageViewModel = new AddBookPageViewModel();
            MakeListPageViewModel makeListPageViewModel = new MakeListPageViewModel();
            MakeListPage makeListPage = new MakeListPage(makeListPageViewModel);
            AddBookPage addBookPage = new AddBookPage(addBookPageViewModel);
            //Not sure if this violates MVVM, but it needs them to navigate between pages.
            //One way to fix this could maybe be to simple have to codebehind handle the navigation
            //but I am not sure if it violates MVVM.
            MainPageViewModel mainPageViewModel = new MainPageViewModel(addBookPage, makeListPage);
            MainPage mainPage = new MainPage(mainPageViewModel);
            NavigationPage navigationPage = new NavigationPage(mainPage)
            {
                BarBackgroundColor = Color.FromHex("#2199e8"),
                BarTextColor = Color.White
            };
            MainPage = navigationPage;

        }
        //I do not know what to do with these.
		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
