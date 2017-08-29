using System.Collections.Generic;

namespace PizzaWebshopCore2.Models.Entities
{
    public class OrderedDish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int OrderId { get; set; }
        public Order Order  { get; set; }
        public List<OrderedDishIngredient> OrderedDishesIngredients { get; set; }
        
    }
}
