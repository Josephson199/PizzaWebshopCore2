using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebshopCore2.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<OrderedDish> OrderedDishes { get; set; }
        public int Price { get; set; }
    }
}
