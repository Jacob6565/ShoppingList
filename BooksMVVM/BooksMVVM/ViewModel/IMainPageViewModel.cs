using BooksMVVM.Model;
using System.Windows.Input;
using Xamarin.Forms;

namespace BooksMVVM.ViewModel
{
    public interface IMainPageViewModel
    {
        double TotalAmount { get; set; }
        INavigation Navigation { get; set; }
        ICommand ToolbarItem_ADD_Command { get; set; }
        ICommand ToolbarItem_FILL_Command { get; set; }
        ICommand ClearBtn_Command { get; set; }
        ICommand DeleteModeBtn_Command { get; set; }
        Book SelectedItem { get; set; }
        void UpdateLocalBooks();


    }
}
