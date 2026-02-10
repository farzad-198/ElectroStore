namespace ElectroStore.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public decimal? OldPrice { get; set; }
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public int Rating { get; set; }
    public bool IsNew { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;
}
