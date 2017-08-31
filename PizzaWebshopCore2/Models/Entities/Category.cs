using System.Collections.Generic;

namespace PizzaWebshopCore2.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Dish> Dishes { get; set; }
    }
}