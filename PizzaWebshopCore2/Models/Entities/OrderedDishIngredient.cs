using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebshopCore2.Models.Entities
{
    public class OrderedDishIngredient
    {
        public int OrderedDishId { get; set; }
        public OrderedDish OrderedDish { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
