using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaWebshopCore2.Data;
using PizzaWebshopCore2.Models.Entities;
using PizzaWebshopCore2.Models.Manage;

namespace PizzaWebshopCore2.Controllers
{
    [Route("manage")]
    [Authorize]
    public class ManageController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ManageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel
            {
                Dishes = _context.Dishes.Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient).Include(d => d.Category).ToList(),
                CreateDishModel = new CreateDishModel
                {
                    CheckBoxIngredient = _context.Ingredients.Select(i => new CheckBoxItem
                    {
                        Id = i.Id,
                        Name = i.Name
                    }).ToList(),
                    Categories = new SelectList(_context.Categories.Select(x => new { Id = x.Id, Value = x.Name }), "Id", "Value")
                }
            };

            return View(model);
        }

        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();      
            }
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Route("edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var dish = await _context.Dishes.Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient).Include(d => d.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }
            var model = new EditViewModel
            {
                Dish = dish,
                CheckBoxIngredient = _context.Ingredients.Select(i => new CheckBoxItem
                {
                    Id = i.Id,
                    Name = i.Name
                }).ToList(),
                Categories = new SelectList(_context.Categories.Select(x => new { Id = x.Id, Value = x.Name }), "Id", "Value")
            };

            return View(model);
        }

        [Route("edit-dish")]
        [HttpPost]
        public async Task<IActionResult> EditDish(EditViewModel editModel)
        {
            var dish = await _context.Dishes.Include(d => d.DishIngredients).SingleOrDefaultAsync(m => m.Id == editModel.Dish.Id);

            if (dish == null)
            {
                return NotFound();
            }

            dish.Name = editModel.Dish.Name;
            dish.Price = editModel.Dish.Price;
            dish.Category = _context.Categories.Find(editModel.CategoryId);

            foreach (var dishDishIngredient in dish.DishIngredients)
            {
                 _context.Remove(dishDishIngredient);
            }

            await _context.SaveChangesAsync();

            foreach (var chIngredientId in editModel.CheckBoxIngredient.Where(ch => ch.IsChecked).Select(ch => ch.Id))
            {
                var dishIngredient = new DishIngredient
                {
                    Dish = dish,
                    Ingredient = _context.Ingredients.Find(chIngredientId)
                };

                _context.Add(dishIngredient);
            }

            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
          
        }

        [Route("create-dish")]
        [HttpPost]
        public async Task<IActionResult> CreateDish(IndexViewModel model)
        {
            var dish = new Dish
            {
                Name = model.CreateDishModel.Name,
                Price = model.CreateDishModel.Price,
                Category = _context.Categories.Find(model.CreateDishModel.CategoryId)
            };

            var dishIngredients = model.CreateDishModel.CheckBoxIngredient.Where(cb => cb.IsChecked)
                .Select(cb => cb.Id)
                .Select(ingredientId => new DishIngredient
                {
                    Dish = dish,
                    Ingredient = _context.Ingredients.Find(ingredientId)
                })
                .ToList();

            dish.DishIngredients = dishIngredients;

            await _context.AddAsync(dish);

            await _context.SaveChangesAsync();
            

            return RedirectToAction(nameof(Index));

        }
    }
}
