using ElectroStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ElectroStore.Models;
using ElectroStore.Extensions;
using ElectroStore.ViewModels;
using System.Threading.Tasks;

namespace ElectroStore.Controllers;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    public CartController(ApplicationDbContext dbContext)
    {
        _context = dbContext;

    }
    // baraye bar resi modam ke kar bar login hast ya na 
    private bool IsloggedIn
    {
        get
        {
            if (User.Identity == null)
                return false;
            else
            {
                return User.Identity.IsAuthenticated;
            }
        }
    }
    private List<CartItem> GetCart()
    {
        if (!IsloggedIn)
        {
            //agar kar bar login nabod az session item hae sabat kharid ra bargardanad
            //agar sabat kharid khali bod 1 list khali az cart item bar gardanad 
            return HttpContext.Session.Get<List<CartItem>>("CART") ?? new();
        }
        else
        {
            //in metod sabate kharid ro be ma bar migardone ke
            //age kar bar login nabashe as session khonde mishe listi az cart item ro bar migardone
            //vali age kar bar login bashe dare sabate kharid ro to data base zakhire mikone 
            var userId = User.GetUserId();
            return _context.UserCartItems
                .Where(c => c.UserId == userId)
                .Select(c => new CartItem
                {
                    Product = c.Product,
                    Quantity = c.Quantity,
                }
                ).ToList();

        }

    }

    public IActionResult Index()
    {
        CartVM _cartVM = new CartVM
        {
            Items = GetCart(),
        };
        return View(_cartVM);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var product = _context.Products.Find(productId);
        if (!IsloggedIn)
        {
            var cart = GetCart();//list item ha ro bar migardone
            var item = cart.FirstOrDefault(i => i.Product.Id == productId);
            if (item == null)
            {
                cart.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                }
                );
            }
            else
            {
                item.Quantity += quantity;
            }
            HttpContext.Session.Set("CART", cart);
            return RedirectToAction("Index");
        }
        else
        {
            var userId = User.GetUserId();
            var item = _context.UserCartItems.FirstOrDefault(i => i.UserId == userId && i.ProductId == productId);
            if (item == null)
            {
                _context.UserCartItems.Add(new UserCartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,

                });


            }
            else
            {
                item.Quantity += quantity;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult Update(int productId, int quantity)
    {
        if (quantity <= 0)
        {
            return Remove(productId);
        }
        if (!IsloggedIn)
        {

            var cart = GetCart();
            var product = cart.FirstOrDefault(p => p.Product.Id == productId);
            if (product != null)
            {
                product.Quantity = quantity;
                HttpContext.Session.Set("CART", cart);


            }
            return RedirectToAction("Index");

        }
        else 
        {
            var userId = User.GetUserId();// be classe user extension eshare mikone 
                var product=_context.UserCartItems.FirstOrDefault(p => p.UserId == userId && p.ProductId == productId);
            if (product != null)
            { 
            product.Quantity = quantity;
                _context.SaveChanges();


            
            }
            return RedirectToAction("Index");

        
        
        }

    }

    [HttpPost]
    public IActionResult Remove(int productId)
    {
        if (!IsloggedIn) 
        {
            var cart = GetCart();
            cart.RemoveAll(p => p.Product.Id==productId);
            HttpContext.Session.Set("CART", cart);

            return RedirectToAction("Index");
        }
       var userId= User.GetUserId();
        var product= _context.UserCartItems.FirstOrDefault(p =>p.UserId == userId && p.ProductId == productId);
        if (product != null)
        {
            _context.UserCartItems.Remove(product);
            _context.SaveChanges();

        
        }
        return RedirectToAction("Index");
        
        
    }


   
}
