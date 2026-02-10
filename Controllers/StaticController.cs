using ElectroStore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ElectroStore.Controllers;

public class StaticController : Controller
{
    private readonly IEmailSender _emailSender;
    

    public StaticController(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }
    public IActionResult About() => View();

    [HttpGet]
    public IActionResult Contact() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(string name, string email, string message)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email)||string.IsNullOrEmpty(message))
        {
            ViewBag.ErorrMessage = "All Fild Are Required";
            return View();
        }
        var subject = $"📩 New Contact Message from {name}";
        var body = $@"
            <h2>Contact Form Message</h2>
            <hr/>
            <p><strong>Name:</strong> {name}</p>
            <p><strong>Email:</strong> {email}</p>
            <p><strong>Message:</strong></p>
            <p>{message}</p>
        ";
        try
        {
            var AdminEmail = "farzadjafarzadehtey@gmail.com";
            await _emailSender.SendEmailAsync(AdminEmail, subject, body);
            ViewBag.SuccessMessage =
     "Your message has been sent successfully. We will contact you soon.";

        }
        catch (Exception)
        {
            ViewBag.ErrorMessage =
      "An error occurred while sending your message. Please try again later.";
           
        }

        return View();
    
    }
    
}
