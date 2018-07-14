using System.Windows.Input;

namespace BooksMVVM.ViewModel
{
    public interface IAddProductPageViewModel
    {
        void UpdateLocalProducts();
        string NameOfProductToAdd { get; set; }
        string ShopOfProductToAdd { get; set; }
        string PriceOfProductToAdd { get; set; }
        ICommand AddProductCommand { get; set; }
        ICommand DeleteProductCommand { get; set; }
    }   
}
