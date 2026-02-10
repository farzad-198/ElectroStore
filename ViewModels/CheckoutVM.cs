using System.ComponentModel.DataAnnotations;

namespace ElectroStore.ViewModels;

public class CheckoutVM
{
    [Required]
    public string FullName { get; set; } = "";

    [Required]
    public string Phone { get; set; } = "";

    [Required]
    public string Address { get; set; } = "";


}
