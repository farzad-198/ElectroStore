using ElectroStore.Models;

namespace ElectroStore.ViewModels;

public class ProductListVM
{
    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();

    public int? SelectedCategoryId { get; set; }
    public decimal MaxPrice { get; set; }
    public string Sort { get; set; } = "newest";
}
