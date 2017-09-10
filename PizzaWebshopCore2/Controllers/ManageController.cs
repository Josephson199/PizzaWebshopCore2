using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaWebshopCore2.Models.Entities;
using PizzaWebshopCore2.Models.Manage;
using PizzaWebshopCore2.Services;

namespace PizzaWebshopCore2.Controllers
{
    [Route("manage")]
    [Authorize]
    public class ManageController : Controller
    {
        private readonly IDatabaseService _databaseService;

        public ManageController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel
            {
                Dishes = _databaseService.GetAll<Dish>().Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient).Include(d => d.Category).ToList(),
                CreateDishModel = new CreateDishModel
                {
                    CheckBoxIngredient = _databaseService.GetAll<Ingredient>().Select(i => new CheckBoxItem
                    {
                        Id = i.Id,
                        Name = i.Name
                    }).ToList(),
                    Categories = new SelectList(_databaseService.GetAll<Category>().Select(x => new { Id = x.Id, Value = x.Name }), "Id", "Value")
                }
            };

            return View(model);
        }

        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dish = await _databaseService.GetAsync<Dish>(id);
            if (dish == null)
            {
                return NotFound();      
            }

            _databaseService.Remove(dish);
            await _databaseService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Route("edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var dish = await _databaseService.GetAll<Dish>().Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient).Include(d => d.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }
            var model = new EditViewModel
            {
                Dish = dish,
                CheckBoxIngredient = _databaseService.GetAll<Ingredient>().Select(i => new CheckBoxItem
                {
                    Id = i.Id,
                    Name = i.Name
                }).ToList(),
                Categories = new SelectList(_databaseService.GetAll<Category>().Select(x => new { Id = x.Id, Value = x.Name }), "Id", "Value")
            };

            return View(model);
        }

        [Route("edit-dish")]
        [HttpPost]
        public async Task<IActionResult> EditDish(EditViewModel editModel)
        {
            var dish = await _databaseService.GetAll<Dish>().Include(d => d.DishIngredients).SingleOrDefaultAsync(m => m.Id == editModel.Dish.Id);

            if (dish == null)
            {
                return NotFound();
            }

            dish.Name = editModel.Dish.Name;
            dish.Price = editModel.Dish.Price;
            dish.Category = await _databaseService.GetAsync<Category>(editModel.CategoryId);

            foreach (var dishIngredient in dish.DishIngredients)
            {
                 _databaseService.Remove(dishIngredient);
            }

            await _databaseService.SaveChangesAsync();

            foreach (var chIngredientId in editModel.CheckBoxIngredient.Where(ch => ch.IsChecked).Select(ch => ch.Id))
            {
                var dishIngredient = new DishIngredient
                {
                    Dish = dish,
                    Ingredient = await _databaseService.GetAsync<Ingredient>(chIngredientId)
                };
                
               _databaseService.Add(dishIngredient);
            }

            await _databaseService.SaveChangesAsync();
            
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
                Category = await _databaseService.GetAsync<Category>(model.CreateDishModel.CategoryId)
            };

            

            var dishIngredients = model.CreateDishModel.CheckBoxIngredient.Where(cb => cb.IsChecked)
                .Select(cb => cb.Id)
                .Select(ingredientId => new DishIngredient
                {
                    Dish = dish,
                    Ingredient = _databaseService.Get<Ingredient>(ingredientId)
                })
                .ToList();

            dish.DishIngredients = dishIngredients;

            await _databaseService.AddAsync(dish);

            await _databaseService.SaveChangesAsync();
            

            return RedirectToAction(nameof(Index));
        }

        [Route("create-ingredient")]
        [HttpPost]
        public async Task<IActionResult> CreateIngredient(IndexViewModel model)
        {
            var ingredient = new Ingredient
            {
                Name = model.CreateIngredientModel.Name,
                Price = model.CreateIngredientModel.Price
            };

            await _databaseService.AddAsync(ingredient);

            await _databaseService.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [Route("create-category")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(IndexViewModel model)
        {
            var category = new Category
            {
                Name = model.CreateCategoryModel.Name
            };

            await _databaseService.AddAsync(category);

            await _databaseService.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

       
    }

}
