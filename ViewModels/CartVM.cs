using ElectroStore.Models;

namespace ElectroStore.ViewModels;

public class CartVM
{
    public List<CartItem> Items { get; set; } = new();

    public decimal TotalPrice =>
        Items.Sum(i => i.Quantity * i.Product.Price);
}
