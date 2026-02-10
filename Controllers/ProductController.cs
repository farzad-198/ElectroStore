using ElectroStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ElectroStore.Models;
using ElectroStore.ViewModels;

namespace ElectroStore.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index(int? categoryId, decimal? maxPrice, string? sort)
    {
        var products = _context.Products.Include(p => p.Category).AsQueryable();
        if (categoryId.HasValue)
        {
        products=products.Where(p => p.CategoryId == categoryId.Value);

        }
        if (maxPrice.HasValue && maxPrice.Value > 0)
        {
            products = products.Where(p => p.Price <= maxPrice.Value);
        }
        if (sort == "priceAsc")
        {
            products = products.OrderBy(p => p.Price);

        }
        else if (sort == "priceDesc")
        {
            products = products.OrderByDescending(p => p.Price);
        }
        else if (sort == "name")
        {
            products = products.OrderBy(p => p.Name);
        }
        else 
        {
        products=products.OrderByDescending(p => p.Id);
        
        }
        ProductListVM productsVM = new ProductListVM
        {
            Products = products.ToList(),
            Categories = _context.Categories.ToList(),
            SelectedCategoryId = categoryId,
            // ?? yani biya negah kon maxpris == null bashe megdaresh ro 0 bezar age meghdar dasht meghdaresho bezar .
            MaxPrice = maxPrice??0,
            Sort=sort?? "newest"
        };

            return View(productsVM);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var product=_context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id==id);
        if (product==null)
        {
            return NotFound();
        }
        return View(product);
    }
    
    
}
