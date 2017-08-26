using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PizzaWebshopCore2.Data;
using PizzaWebshopCore2.Models.Dishes;

namespace PizzaWebshopCore2.Controllers
{
    [Route("dishes")]
    public class DishesController : Controller
    {
        private const string SessionKeyName = "_Cart";
        private readonly ApplicationDbContext _context;
        

        public DishesController(ApplicationDbContext context)
        {
           
            _context = context;
        }

        public IActionResult Index()
        {
            var dishes = _context.Dishes
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient);

            var dishesTransformed = dishes.Select(d => new DishModel
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price,
                Ingredients = d.DishIngredients.Select(di => new IngredientModel
                {
                    Name = di.Ingredient.Name,
                    Price = di.Ingredient.Price

                }).ToList()
                
            }).ToList();

            var ingredients = _context.Ingredients.ToList();

            var ingredientsTransformed = ingredients.Select(i => new IngredientModel
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price
            }).ToList();

            var viewModel = new IndexViewModel
            {
                Dishes = dishesTransformed,
                Ingredients = ingredientsTransformed
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("checkout")]
        public JsonResult Checkout([FromBody] OrderJson orderJson )
        {

            return Json("success");
        }

        [HttpPost]
        [Route("add-dish-to-session")]
        public JsonResult AddDishToSession([FromBody] int id)
        {
            var dishModel = _context.Dishes
                .Select(d => new DishModel
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price,
                Ingredients = d.DishIngredients.Select(di => new IngredientModel
                {
                    Id = di.Ingredient.Id,
                    Name = di.Ingredient.Name,
                    Price = di.Ingredient.Price
                }).ToList()
            })
            .FirstOrDefault(d => d.Id == id);

            if (dishModel == null)
            {
                return Json("fail");
            }
            
            AddToSession(dishModel);
          
            return Json("success");
        }

        [HttpPost]
        [Route("add-custom-dish-to-session")]
        public JsonResult AddCustomDishToSession([FromBody] int[] ingredientIds)
        {
            if (ingredientIds == null)
            {
                return Json("fail");
            }
           
            var ingredientsModel = _context.Ingredients.Where(i => ingredientIds.Contains(i.Id))
                .Select(i => new IngredientModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price
                }).ToList();

            var dishModel = new DishModel
            {
                Id = new Random().Next(),
                Name = "Custom",
                Ingredients = ingredientsModel

            };

           AddToSession(dishModel);
           return Json("success");

        }

        private void AddToSession(DishModel dishModel)
        {

            var cartSession = HttpContext.Session.GetString(SessionKeyName);

            if (cartSession == null)
            {
                var cart = new Cart
                {
                    Dishes = new List<DishModel> { dishModel }

                };

                var serializedCart = JsonConvert.SerializeObject(cart);

                HttpContext.Session.SetString(SessionKeyName, serializedCart);
            }
            else
            {
                var cartSessionDeserialized = JsonConvert.DeserializeObject<Cart>(cartSession);
                cartSessionDeserialized.Dishes.Add(dishModel);

                var serializedCart = JsonConvert.SerializeObject(cartSessionDeserialized);

                HttpContext.Session.SetString(SessionKeyName, serializedCart);
            }

            
        }
    }
}