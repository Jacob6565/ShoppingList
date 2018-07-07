using System.Windows.Input;

namespace BooksMVVM.ViewModel
{
    public interface IAddBookPageViewModel
    {
        void UpdateLocalBooks();
        string NameOfBookToAdd { get; set; }
        string ShopOfBookToAdd { get; set; }
        string PriceOfBookToAdd { get; set; }
        ICommand AddBookCommand { get; set; }
        ICommand DeleteBookCommand { get; set; }
    }   
}
