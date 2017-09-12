using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PizzaWebshopCore2.Models.Manage;

namespace PizzaWebshopCore2.Models.Account
{
    public class EditUserModel
    {
        public UserModel User { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<CheckBoxRole> CheckboxRoles { get; set; }
    }

    public class CheckBoxRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
}
