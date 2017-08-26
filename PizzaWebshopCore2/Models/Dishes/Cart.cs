using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebshopCore2.Models.Dishes
{
    public class Cart
    {
        public List<DishModel> Dishes { get; set; }

        public int CartPrice    
        {
            get
            {
                return Dishes.Sum(dishModel => dishModel.Price);
            }
            
        }
    }
}
