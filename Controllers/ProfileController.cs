using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ElectroStore.Data;
using ElectroStore.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ElectroStore.Controllers;

public class ProfileController : Controller
{
    UserManager<ApplicationUser> _userManager;
    ApplicationDbContext _ApplicationDb;
    public ProfileController(UserManager<ApplicationUser> manager, ApplicationDbContext dbContext)
    {
        _userManager = manager;
        _ApplicationDb = dbContext;

    }
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }
        ViewBag.FullName = user.FullName;
        ViewBag.Email = user.Email;
        ViewBag.IsProfileComplete = user.IsProfileComplete;
        var orders = await _ApplicationDb.Orders.Where(o => o.UserId == user.Id).ToListAsync();

        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> Complete()
    {
        var user = await _userManager.GetUserAsync(User);

        return View(user);
    }
    [HttpPost]
    public async Task<IActionResult> Complete(ApplicationUser application)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }
        user.FullName = application.FullName;
        user.PhoneNumber = application.PhoneNumber;
        user.Address = application.Address;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
         ModelState.AddModelError("","update is not succsesfully");
            return View(application);
        }

        return RedirectToAction("index");
    }

    public async Task<IActionResult> Orders()
    { 
    var user=await _userManager.GetUserAsync(User);
        if (user == null) 
        {
            return NotFound();
        }
        var orders = await _ApplicationDb.Orders.Where(o => o.UserId == user.Id).ToListAsync();
        return View(orders);
    }

    public async Task<IActionResult> OrderDetailAsync(int id)
    { 
     var user=await _userManager.GetUserAsync(User);
        if (user == null) 
        {
            return NotFound();
        }
        var order = await _ApplicationDb.Orders.FirstOrDefaultAsync(o => o.Id==id&& o.UserId==user.Id );
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    
    
    }
}
