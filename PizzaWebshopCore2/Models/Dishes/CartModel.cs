﻿using System.Collections.Generic;
using System.Linq;

namespace PizzaWebshopCore2.Models.Dishes
{
    public class CartModel
    {
        public List<DishModel> Dishes { get; set; }

        public int CartPrice    
        {
            get
            {
                var total = 0;
                if (Dishes != null && Dishes.Any())
                {
                    foreach (var dish in Dishes)
                    {
                        if (dish.Price <= 0)
                        {
                            total += dish.IngredientCosts;
                        }
                        else
                        {
                            total += dish.Price;
                        }
                    }
                }
               
                return total;
            }
            
        }
    }
}
