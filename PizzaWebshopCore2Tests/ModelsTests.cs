using System.Collections.Generic;
using PizzaWebshopCore2.Models.Dishes;
using Xunit;

namespace PizzaWebshopCore2Tests
{
    public class ModelsTests
    {

        public CartModel CreateCartModel()
        {
            const int ingredientPrice = 10;
            const int dishPrice = 20;

            return new CartModel
            {
                Dishes = new List<DishModel>
                {
                    new DishModel
                    {
                        Price = dishPrice,
                        Ingredients = new List<IngredientModel>
                        {
                            new IngredientModel
                            {
                                Price = ingredientPrice
                            }
                        }
                    },
                    new DishModel
                    {
                        Ingredients = new List<IngredientModel>
                        {
                            new IngredientModel
                            {
                                Price = ingredientPrice
                            }
                        }
                    }
                }
            };
        }

        [Fact]
        public void Test_CartModel_Price_Calc()
        {
            //arrange

            var cartModel = CreateCartModel();
          
            
            //act
            //assert

            Assert.Equal(30, cartModel.CartPrice);
        }
        
    }
}
