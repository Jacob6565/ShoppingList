using System;

namespace BooksMVVM.Model
{
    public interface IProduct : IComparable, IEquatable<IProduct>
    {
        int ID { get; set; }
        string Name { get; set; }
        string NameAndPrice { get; set; }
        string Shop { get; set; }
        double Price { get; set; }
        bool IsVisible { get; set; }
    }
}
