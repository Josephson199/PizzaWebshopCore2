using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PizzaWebshopCore2.Models.Account
{
    public class EditUsersModel
    {
        public List<UserModel> Users { get; set; }
    }

    public class UserModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
