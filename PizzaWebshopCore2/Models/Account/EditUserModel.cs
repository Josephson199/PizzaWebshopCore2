using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PizzaWebshopCore2.Models.Account
{
    public class EditUserModel
    {
        public UserModel User { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}
