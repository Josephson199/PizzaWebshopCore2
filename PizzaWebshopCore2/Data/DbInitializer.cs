using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using PizzaWebshopCore2.Models;
using PizzaWebshopCore2.Models.Entities;

namespace PizzaWebshopCore2.Data
{
    public class DbInitializer
    {
        public static void Initialize(UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            var aUser = new ApplicationUser
            {
                UserName = "student@test.com",
                Email = "student@test.com"
            };
            var userResult = userManager.CreateAsync(aUser, "Pa$$w0rd").Result;

            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            var adminUser = new ApplicationUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com"
            };
            var adminUserResult = userManager.CreateAsync(adminUser, "Pa$$w0rd").Result;

            var roleAddedResult = userManager.AddToRoleAsync(adminUser, "Admin").Result;

            if (!context.Dishes.Any())
            {
                var cheese = new Ingredient { Name = "Cheese" };
                var tomato = new Ingredient { Name = "Tomato" };
                var ham = new Ingredient { Name = "Ham" };

                var capricciosa = new Dish { Name = "Capricciosa", Price = 80 };
                var margarita = new Dish { Name = "Margarita", Price = 70 };
                var hawaii = new Dish { Name = "Hawaii", Price = 100 };

                var margaritaCheese = new DishIngredient { Dish = margarita, Ingredient = cheese };
                var margaritaHam = new DishIngredient { Dish = margarita, Ingredient = ham };

                var hawaiiCheese = new DishIngredient { Dish = hawaii, Ingredient = tomato };

                var capricciosaCheese = new DishIngredient { Dish = capricciosa, Ingredient = ham };
                var capricciosaTomato = new DishIngredient { Dish = capricciosa, Ingredient = tomato };


                capricciosa.DishIngredients = new List<DishIngredient> { capricciosaCheese, capricciosaTomato };


                margarita.DishIngredients = new List<DishIngredient> { margaritaCheese, margaritaHam };
                hawaii.DishIngredients = new List<DishIngredient> { hawaiiCheese };

                context.AddRange(tomato, ham, cheese);

                //context.DishIngredients.AddRange(margaritaCheese, hawaiiCheese, capricciosaCheese, capricciosaTomato, margaritaHam);
                context.AddRange(capricciosa, margarita, hawaii);
                context.SaveChanges();
            }
        }
    }
}
