using ElectroStore.Models;
using ElectroStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElectroStore.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    public AccountController(UserManager<ApplicationUser> manager, RoleManager<IdentityRole> role, SignInManager<ApplicationUser> signIn, IEmailSender emailSender)
    {
        _userManager = manager;
        _roleManager = role;
        _signInManager = signIn;
        _emailSender = emailSender;

    }
    [HttpGet]
    public async Task<IActionResult> Login()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || string.IsNullOrEmpty(user.UserName))
        {
            ModelState.AddModelError("", "Email Or Password Is Not Valid");
            return View(model);
        }
        var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Email Or Password Is Not Valid");
            return View(model);
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var Adduser = new ApplicationUser { FullName = model.FullName, Email = model.Email, UserName = model.Email };
        var result = await _userManager.CreateAsync(Adduser, model.Password);
        if (!result.Succeeded)
        {
            return View(model);
        }
        if (!await _roleManager.RoleExistsAsync("Customer"))
            await _roleManager.CreateAsync(new IdentityRole("Customer"));
        await _userManager.AddToRolesAsync(Adduser, new[] { "Customer" });
        await _signInManager.SignInAsync(Adduser, isPersistent: false);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public async Task<IActionResult> ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            ViewBag.Message = "Plase Enter Your Email";
            return View();
        }
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || string.IsNullOrEmpty(user.Email))
        {
            ViewBag.Message = " Your UserName OR Email Is Not Exists";
            return View();
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);
        var body = $@"
<p>Hello {user.FullName},</p>
<p>Click the link below to reset your password:</p>
<p>
    <a href='{resetLink}'>Reset Password</a>
</p>
<p>If you did not request this, please ignore this email.</p>
";
        await _emailSender.SendEmailAsync(user.Email!, "Restart Password", body);

        ViewBag.Message = "Password Restart Link Has been send";
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ResetPassword(string token, string email)
    {
        if (string.IsNullOrEmpty(token)||string.IsNullOrEmpty(email)) 
        {

            return RedirectToAction("Login");
        }
        ViewBag.Token=token;
        ViewBag.Email = email;


        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(string token, string email, string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            ViewBag.Message = "Password Is Required";
            ViewBag.Token=token.ToString();
            ViewBag.Email=email;
            return View();
        }
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)   
        {
            ViewBag.Message = "Email Is Not Valid";
            return View();
        }
        var result=await _userManager.ResetPasswordAsync(user,token,password);
        if (!result.Succeeded)
        {
            ViewBag.Message = "Request is not valid";
            ViewBag.Token = token;
            ViewBag.Email = email;
            return View();
        
        }
        ViewBag.Message = "Password Reset Is Succsesfully";

        return View();
    }
}