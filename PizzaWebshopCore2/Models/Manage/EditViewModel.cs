using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzaWebshopCore2.Models.Entities;

namespace PizzaWebshopCore2.Models.Manage
{
    public class EditViewModel
    {
        public Dish Dish { get; set; }
        public List<CheckBoxItem> CheckBoxIngredient { get; set; }
    }
    public class CheckBoxItem
    {
        public bool IsChecked { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
