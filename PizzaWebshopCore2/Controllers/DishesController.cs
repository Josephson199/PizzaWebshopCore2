using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaWebshopCore2.Data;
using PizzaWebshopCore2.Models.Dishes;

namespace PizzaWebshopCore2.Controllers
{
    [Route("dishes")]
    public class DishesController : Controller
    {

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
                Ingredients = d.DishIngredients.Select(di => new IngredientModel
                {
                    Name = di.Ingredient.Name

                }).ToList()
                
            }).ToList();

            var ingredients = _context.Ingredients.ToList();

            var ingredientsTransformed = ingredients.Select(i => new IngredientModel
            {
                Id = i.Id,
                Name = i.Name
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
    }

    public class Person
    {
        public string Name { get; set; }
        public List<Beer> Beers { get; set; }
    }

    public class Beer
    {
        public string Name { get; set; }
    }
}