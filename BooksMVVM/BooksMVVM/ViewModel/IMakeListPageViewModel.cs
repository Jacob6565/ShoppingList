using BooksMVVM.Model;
using System.Windows.Input;

namespace BooksMVVM.ViewModel
{
    public interface IMakeListPageViewModel
    {
        void UpdateLocalProducts();
        Product SelectedItem { get; set; }
        ICommand SortByName_Command { get; set; }
        ICommand SortByShop_Command { get; set; }
    }
}
