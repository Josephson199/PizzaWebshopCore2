using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzaWebshopCore2.Models.Entities;

namespace PizzaWebshopCore2.Models.Manage
{
    public class EditViewModel
    {
        public Dish Dish { get; set; }
        public List<CheckBoxItem> CheckBoxIngredient { get; set; }
        public int CategoryId { get; set; }
        public SelectList Categories { get; set; }
    }
}
