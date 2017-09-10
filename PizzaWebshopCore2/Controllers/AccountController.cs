using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaWebshopCore2.Models;
using PizzaWebshopCore2.Models.Account;


namespace PizzaWebshopCore2.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
           
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Dishes");
                }
                
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Street = model.Street,
                    City = model.City,
                    PostalCode = model.PostalCode
                };

                var userResult = await _userManager.CreateAsync(newUser, model.Password);
                if (userResult.Succeeded)
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    return RedirectToAction("Index", "Dishes");

                }
                foreach (var error in userResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid register attempt.");
            }

            
            return View(model);
        }

        [Route("users")]
        public async Task<IActionResult> EditUsers()
        {
            var model = new EditUsersModel
            {
                Users = new List<UserModel>()
            };

            var applicationUsers =  _userManager.Users.ToList();

            foreach (var applicationUser in applicationUsers)
            {
                var userModel = new UserModel
                {
                    Roles = await _userManager.GetRolesAsync(applicationUser),
                    Email = applicationUser.Email,
                    City = applicationUser.City,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    Id = applicationUser.Id,
                    PostalCode = applicationUser.PostalCode,
                    Street = applicationUser.Street
                };
                model.Users.Add(userModel);

            }

            return View(model);
        }

        [Route("users/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var applicationUser = _userManager.Users.FirstOrDefault(u => u.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            var userModel = new UserModel
            {
                Roles = await _userManager.GetRolesAsync(applicationUser),
                Email = applicationUser.Email,
                City = applicationUser.City,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Id = applicationUser.Id,
                PostalCode = applicationUser.PostalCode,
                Street = applicationUser.Street
            };

            var model = new EditUserModel
            {
                User = userModel,
                Roles = await _roleManager.Roles.ToListAsync()
            };
            
            return View(model);
        }
    }
}
