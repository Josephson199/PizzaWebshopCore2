using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PizzaWebshopCore2.Data;
using PizzaWebshopCore2.Models.Dishes;
using PizzaWebshopCore2.Models.Entities;
using PizzaWebshopCore2.Services;
using Xunit;

namespace PizzaWebshopCore2Tests
{
    public class TransformationServiceTests
    {
        private readonly IServiceProvider _serviceProvider;

        public TransformationServiceTests()
        {
            var efServiceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(b =>
                b.UseInMemoryDatabase("DefaultConnection")
                    .UseInternalServiceProvider(efServiceProvider));

            //Add services here
            services.AddTransient<TransformationService>();

            _serviceProvider = services.BuildServiceProvider();
        }
       
        [Fact]
        public void TestDishesToDishesModelTransformation()
        {
            
            //arrange
            var sut = _serviceProvider.GetService<TransformationService>();
            
            var listOfDishes = new List<Dish>();

            var dish = new Dish
            {
                Name = "test",
                Price = 50,
                Category = new Category
                {
                    Name = "categoryTest"
                },
                DishIngredients = new List<DishIngredient>
                {
                    new DishIngredient
                    {
                        Ingredient = new Ingredient
                        {
                            Name = "ingredientTest",
                            Price = 20,
                            Id = 2
                        }
                    }
                }
            };

            listOfDishes.Add(dish);
            //act

            var result = sut.TranformDishesToDishModels(listOfDishes);
            //asserts
            Assert.NotNull(result);
            Assert.IsType<List<DishModel>>(result);
            Assert.InRange(result.Count, 1, 1);
            Assert.InRange(result[0].Ingredients.Count, 1,1);

            
        }
    }
}
