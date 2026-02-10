namespace ElectroStore.Models;

public class CartItem
{
    //sabate kharid afrade ke login nakardan 
    public Product Product { get; set; } = default!;
    public int Quantity { get; set; }
}
