using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzaWebshopCore2.Models.Entities;

namespace PizzaWebshopCore2.Models.Manage
{
    public class IndexViewModel
    {
        public List<Dish> Dishes { get; set; }
        public CreateDishModel CreateDishModel { get; set; }
        public CreateIngredientModel CreateIngredientModel { get; set; }
        public CreateCategoryModel CreateCategoryModel { get; set; }
    }

    public class CreateDishModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public SelectList Categories { get; set; }
        public List<CheckBoxItem> CheckBoxIngredient { get; set; }
    }

    public class CreateIngredientModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
    }
    public class CreateCategoryModel
    {
        [Required]
        public string Name { get; set; }
    }
}
