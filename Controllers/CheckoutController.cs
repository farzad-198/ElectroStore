using Microsoft.AspNetCore.Mvc;
using ElectroStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using ElectroStore.Data;
using ElectroStore.Models;
using ElectroStore.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace ElectroStore.Controllers;

public class CheckoutController : Controller
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    public CheckoutController(UserManager<ApplicationUser> manager, ApplicationDbContext context)

    {
        _userManager = manager;
        _context = context;

    }
    private async Task MergeSessionCart(string userId)
    {
        var cart = HttpContext.Session.Get<List<CartItem>>("CART");
        if (cart == null)
        {
            return;

        }
        foreach (var item in cart)
        {
            var product = _context.UserCartItems
                .FirstOrDefault(u => u.UserId == userId && u.ProductId == item.Product.Id);
            if (product != null)
            {
                product.Quantity += item.Quantity;
            }
            else 
            {
                _context.UserCartItems.Add(new UserCartItem
                {
                    UserId = userId,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity
                });
            }
        }
        _context.SaveChanges();
        HttpContext.Session.Remove("CART");



    }



    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");

        }
        if (!user.IsProfileComplete)
        {
            return RedirectToAction("Complete", "Profile");
        }
        ViewBag.FullName = user.FullName;
        ViewBag.Address = user.Address;
        ViewBag.Phone = user.PhoneNumber;
        var products = _context.UserCartItems.Include(u => u.Product)
            .Where(u => u.UserId == user.Id).ToList();
        ViewBag.TotalPrice = products.Sum(i => i.Quantity * i.Product.Price);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CheckoutVM model)
    {
       var user=await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        
        }
        if (!user.IsProfileComplete)
        {
            return RedirectToAction("Complete", "Profile");
        
        }
        await MergeSessionCart(user.Id);
        var cart = _context.UserCartItems.Include(_ => _.Product)
            .Where(p => p.UserId==user.Id).ToList();
        if (cart == null)
        {
            return RedirectToAction("Index", "Cart");

        }
        else 
        {
            var order=new Order 
            {
                UserId = user.Id,
                Date = DateTime.Now,
                Total=cart.Sum(i =>i.Quantity*i.Product.Price)
            
            };
            _context.Orders.Add(order);
            _context.UserCartItems.RemoveRange(cart);
            _context.SaveChanges();
            return RedirectToAction("Success",new {orderId=order.Id }
            );
        }
            

    }

    [HttpGet]
    public IActionResult Success(int orderId)
    {

        ViewBag.OrderId = orderId;
        return View();

    }
}


