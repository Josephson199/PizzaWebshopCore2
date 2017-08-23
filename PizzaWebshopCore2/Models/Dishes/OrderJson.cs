using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzaWebshopCore2.Models.Entities;

namespace PizzaWebshopCore2.Models.Dishes
{
    public class OrderJson
    {
        public List<DishJson> Dishes { get; set; }
    }

    public class DishJson
    {
        public string Name { get; set; }
        public int? Count { get; set; }
        public List<IngredientJson> Ingredients { get; set; }
    }

    public class IngredientJson
    {
        public string Name { get; set; }        
    }
}
