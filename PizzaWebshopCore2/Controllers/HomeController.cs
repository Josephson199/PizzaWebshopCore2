using Microsoft.AspNetCore.Mvc;
using PizzaWebshopCore2.Models.Home;

namespace PizzaWebshopCore2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(bool orderComplete = false)
        {
            var viewModel = new IndexViewModel();
            if (orderComplete)
            {
                viewModel.CompletedOrder = true;
            }

            return View(viewModel);
        }
    }
}