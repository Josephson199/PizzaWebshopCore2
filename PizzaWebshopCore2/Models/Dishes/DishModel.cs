using System.Collections.Generic;

namespace PizzaWebshopCore2.Models.Dishes
{
    public class DishModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<IngredientModel> Ingredients { get; set; }
    }
}
