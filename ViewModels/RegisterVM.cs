using System.ComponentModel.DataAnnotations;

namespace ElectroStore.ViewModels;

public class RegisterVM
{
    [Required]
    public string FullName { get; set; } = "";

    [Required]
    public string Email { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}
