using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PizzaWebshopCore2.Models.Account
{
    public class EditUsersModel
    {
        public List<UserModel> Users { get; set; }
    }

    public class UserModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
