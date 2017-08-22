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

            var viewModel = new IndexViewModel
            {
                Dishes = dishesTransformed
            };

            return View(viewModel);
        }
    }
}