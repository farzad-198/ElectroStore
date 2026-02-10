using Microsoft.AspNetCore.Identity;

namespace ElectroStore.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = "";
    public string? Address { get; set; }
    //Helper Properte filde mohasebati 
    public bool IsProfileComplete =>
       !string.IsNullOrWhiteSpace(FullName)
       && !string.IsNullOrWhiteSpace(PhoneNumber)
       && !string.IsNullOrWhiteSpace(Address);
}
