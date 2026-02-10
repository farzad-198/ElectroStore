using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectroStore.Controllers;

using ElectroStore.Data;
using ElectroStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;



//[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // =======================
    // DASHBOARD
    // =======================
    public async Task<IActionResult> Index()
    {
        ViewBag.TotalProducts = await _context.Products.CountAsync();
        ViewBag.TotalOrders = await _context.Orders.CountAsync();
        ViewBag.TotalUsers = await _userManager.Users.CountAsync();

        ViewBag.Revenue = await _context.Orders.SumAsync(o => o.Total);


        return View();
    }

    // =========================
    // PRODUCTS - LIST
    // =========================
    public async Task<IActionResult> Products()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync();

        return View(products);
    }

    // =========================
    // CREATE - GET
    // =========================
    [HttpGet]
    public IActionResult ProductCreate()
    {

        ViewBag.Categories = new SelectList(
            _context.Categories.ToList(),
            "Id",
            "Name"
        );

        return View();
    }

    // =========================
    // CREATE - POST
    // =========================
    [HttpPost]
    public async Task<IActionResult> ProductCreate(Product product, IFormFile imageFile)
    {

        if (imageFile != null && imageFile.Length > 0)
        {
            var uploadPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "products"
            );

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            product.ImageUrl = "/images/products/" + fileName;
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Products));
    }


    // =========================
    // EDIT - GET
    // =========================
    [HttpGet]
    public async Task<IActionResult> ProductEdit(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        ViewBag.Categories = new SelectList(
            _context.Categories.ToList(),
            "Id",
            "Name",
            product.CategoryId
        );

        return View(product);
    }

    // =========================
    // EDIT - POST
    // =========================
    [HttpPost]
    public async Task<IActionResult> ProductEdit(Product product, IFormFile imageFile)
    {
      

        var dbProduct = await _context.Products.FindAsync(product.Id);
        if (dbProduct == null)
            return NotFound();

        
        dbProduct.Name = product.Name;
        dbProduct.Price = product.Price;
        dbProduct.OldPrice = product.OldPrice;
        dbProduct.Description = product.Description;
        dbProduct.CategoryId = product.CategoryId;
        dbProduct.IsNew = product.IsNew;
        dbProduct.Rating = product.Rating;

        if (imageFile != null && imageFile.Length > 0)
        {
            var uploadPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "products"
            );

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            dbProduct.ImageUrl = "/images/products/" + fileName;
        }

        _context.Products.Update(dbProduct);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Products));
    }


    // =========================
    // DELETE
    // =========================
    [HttpPost]
    public async Task<IActionResult> ProductDelete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Products));
    }
    // =======================
    // CATEGORIES
    // =======================
    public async Task<IActionResult> Categories()
    {
        var categories = await _context.Categories
            .Include(c => c.Products)
            .ToListAsync();

        return View(categories);
    }

    public IActionResult CategoryCreate()
    {
        return View(new Category());
    }

    [HttpPost]
    public async Task<IActionResult> CategoryCreate(Category model)
    {
        

        _context.Categories.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Categories));
    }

    public async Task<IActionResult> CategoryEdit(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> CategoryEdit(Category model)
    {
        _context.Categories.Update(model);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Categories));
    }
    // =======================
    // CATEGORY DELETE
    // =======================

    public async Task<IActionResult> CategoryDelete(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return NotFound();

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Categories));
    }


    // ================= ORDERS =================

    public IActionResult Orders()
    {
        var orders = _context.Orders
            .OrderByDescending(o => o.Date)
            .ToList();

        return View(orders);
    }

    public IActionResult OrderDetail(int id)
    {
        var order = _context.Orders
            .FirstOrDefault(o => o.Id == id);

        if (order == null)
            return NotFound();

        return View(order);
    }


    // =======================
    // USERS
    // =======================


    public async Task<IActionResult> Users()
    {
        var users = _context.Users
            .ToList();

        return View(users);
    }

    public async Task<IActionResult> UserDetail(string id)
    {
        if (id == null)
            return NotFound();

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);


        ViewBag.Role = roles.First();

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> UserDelete(string id)
    {
        if (id == null)
            return NotFound();

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        await _userManager.DeleteAsync(user);

        return RedirectToAction(nameof(Users));
    }
}

