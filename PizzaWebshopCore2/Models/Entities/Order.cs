using System.Collections.Generic;

namespace PizzaWebshopCore2.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string FkApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }    
        public List<DishOrder> DishOrders { get; set; }
    }
}
