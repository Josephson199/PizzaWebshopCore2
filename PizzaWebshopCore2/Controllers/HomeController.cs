using Microsoft.AspNetCore.Mvc;
using PizzaWebshopCore2.Models.Home;

namespace PizzaWebshopCore2.Controllers
{
    
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel();
            return View(viewModel);
        }
    }
}