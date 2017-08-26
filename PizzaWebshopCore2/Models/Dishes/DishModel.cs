using System.Collections.Generic;
using System.Linq;

namespace PizzaWebshopCore2.Models.Dishes
{
    public class DishModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public List<IngredientModel> Ingredients { get; set; }

        public int IngredientCosts
        {
            get { return Ingredients.Sum(i => i.Price); }
        }
    }
}
