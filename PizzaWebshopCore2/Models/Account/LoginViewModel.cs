using System.ComponentModel.DataAnnotations;

namespace PizzaWebshopCore2.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
