﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PizzaWebshopCore2.Data;
using PizzaWebshopCore2.Models;
using PizzaWebshopCore2.Models.Dishes;
using PizzaWebshopCore2.Models.Entities;

namespace PizzaWebshopCore2.Controllers
{
    [Route("dishes")]
    public class DishesController : Controller
    {
        private const string SessionKeyName = "_Cart";
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        

        public DishesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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
                    Id = di.Ingredient.Id,
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
                Ingredients = ingredientsTransformed,
                PaymentInformationModel = new PaymentInformationModel()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("add-dish-to-session")]
        public JsonResult AddDishToSession([FromBody] DishJsonDto dishJsonDto)
        {
            var dishModel = _context.Dishes
                .Select(d => new DishModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Price = d.Price,
                    Ingredients = _context.Ingredients.Where(i => dishJsonDto.IngredientIds.Contains(i.Id))
                    .Select(i => new IngredientModel
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Price = i.Price
                        })
                    .ToList()
                })
            .FirstOrDefault(d => d.Id == dishJsonDto.DishId);

            if (dishModel == null)
            {
                return Json("fail");
            }

            var cart = AddToSession(dishModel);

            var jsonDishResult = new JsonDishResult
            {
                Dish = dishModel,
                CartPrice = cart.CartPrice
            };

            return Json(jsonDishResult);
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
                Ingredients = ingredientsModel,
                Price = ingredientsModel.Sum(i => i.Price)

            };

           var cart = AddToSession(dishModel);
           var jsonDishResult = new JsonDishResult
           {
               Dish = dishModel,
               CartPrice = cart.CartPrice
           };

           return Json(jsonDishResult);

        }

        [HttpGet]
        [Route("get-cart")]
        public JsonResult GetCart()
        {
            var cartSession = HttpContext.Session.GetString(SessionKeyName);
            if (cartSession == null)
            {
                return Json(new CartModel());
            }

            var cart = JsonConvert.DeserializeObject<CartModel>(cartSession);

            return Json(cart);
        }

        [HttpPost]
        [Route("remove-dish-from-cart-session")]
        public JsonResult RemoveDishFromCartSession([FromBody] int id)
        {
            var cartSession = HttpContext.Session.GetString(SessionKeyName);
            if (cartSession == null)
            {
                return Json(new CartModel());
            }

            var cart = JsonConvert.DeserializeObject<CartModel>(cartSession);
            var dishToRemove = cart.Dishes.Find(f => f.Id == id);

            if (dishToRemove == null)
            {
                return Json(new CartModel());
            }

            cart.Dishes.Remove(dishToRemove);


            var serializedCart = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString(SessionKeyName, serializedCart);

            return Json(cart);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("save-order-unauthorized")]
        public async Task<IActionResult> SaveOrderUnauthorized([FromBody] PaymentInformationModel paymentInformationModel)
        {
            var cartSession = HttpContext.Session.GetString(SessionKeyName);

            if (cartSession == null)
            {
                return Json("fail");
            }

            var aUser = new ApplicationUser
            {
                UserName = paymentInformationModel.Email,
                Email = paymentInformationModel.Email,
                FirstName = paymentInformationModel.FirstName,
                LastName = paymentInformationModel.LastName,
                City = paymentInformationModel.City,
                Street = paymentInformationModel.Street,
                PostalCode = paymentInformationModel.PostalCode
            };

            var userResult = await _userManager.CreateAsync(aUser, "Pa$$w0rd");

            if (!userResult.Succeeded)
            {
                return Json("fail");
            }
            
            var result = await _signInManager.PasswordSignInAsync(aUser.Email, "Pa$$w0rd", true, false);

            if (!result.Succeeded)
            {
                return Json("fail");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Json("fail");
            }
            
            var cart = JsonConvert.DeserializeObject<CartModel>(cartSession);

            var order = CreateOrder(cart, user);

            _context.Add(order);
            _context.SaveChanges();

            //email?
         
            HttpContext.Session.Clear();
            return Json("orderComplete");
        }

        [HttpPost]
        [Authorize]
        [Route("save-order-authorized")]
        public async Task<JsonResult> SaveOrderAuthorized([FromBody] PaymentInformationModel paymentInformationModel)
        {
            //get user
            var user = await _userManager.GetUserAsync(User);
            var cartSession = HttpContext.Session.GetString(SessionKeyName);

            if (user == null || cartSession == null)
            {
                return Json("fail");
            }

            var cart = JsonConvert.DeserializeObject<CartModel>(cartSession);

            var order = CreateOrder(cart, user);

            _context.Add(order);
            _context.SaveChanges();

            //email?
            HttpContext.Session.Clear();
            return Json("orderComplete");
        }

        private Order CreateOrder(CartModel cart, ApplicationUser user)
        {
            var order = new Order
            {
                ApplicationUser = user,
                Price = cart.CartPrice
            };

            var orderedDishes = cart.Dishes.Select(d => new OrderedDish
            {
                Name = d.Name,
                Price = d.Price,
                Order = order,
                OrderedDishesIngredients = d.Ingredients.Select(i => new OrderedDishIngredient
                {
                    Ingredient = _context.Ingredients.FirstOrDefault(ie => ie.Id == i.Id)
                }).ToList()

            });

            order.OrderedDishes = orderedDishes.ToList();

            return order;
        }


        private CartModel AddToSession(DishModel dishModel)
        {

            var cartSession = HttpContext.Session.GetString(SessionKeyName);

            CartModel cart;

            if (cartSession == null)
            {
                cart = new CartModel
                {
                    Dishes = new List<DishModel> { dishModel }

                };

                var serializedCart = JsonConvert.SerializeObject(cart);

                HttpContext.Session.SetString(SessionKeyName, serializedCart);
            }
            else
            {
                cart = JsonConvert.DeserializeObject<CartModel>(cartSession);
                cart.Dishes.Add(dishModel);

                var serializedCart = JsonConvert.SerializeObject(cart);

                HttpContext.Session.SetString(SessionKeyName, serializedCart);
            }

            return cart;
        }

    }
}