using System.Collections.Generic;
using PizzaWebshopCore2.Models.Dishes;
using PizzaWebshopCore2.Models.Entities;
using System.Linq;

namespace PizzaWebshopCore2.Services
{
    public class TransformationService
    {
        public IReadOnlyList<DishModel> TranformDishesToDishModels(IEnumerable<Dish> dishes)
        {
            var dishesTransformed = dishes.Select(d => new DishModel
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price,
                Category = new CategoryModel { Name = d.Category.Name },
                Ingredients = d.DishIngredients.Select(di => new IngredientModel
                {
                    Id = di.Ingredient.Id,
                    Name = di.Ingredient.Name,
                    Price = di.Ingredient.Price

                }).ToList()

            }).ToList();

            return dishesTransformed;
        }
    }
}
