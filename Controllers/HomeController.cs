using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectroStore.Data;
using ElectroStore.Models;

namespace ElectroStore.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? query)
    {
        IQueryable<Product> products = _context.Products.Include(x => x.Category);
        if (!string.IsNullOrEmpty(query))
        {

            products = products.Where(x => x.Name.Contains(query) || x.Description.Contains(query)||x.Category.Name.Contains(query));

        }
        var result=await products.OrderByDescending(p => p.IsNew).ThenByDescending(p=>p.Id).Take(8).ToListAsync();

        return View(result);
    }


}
