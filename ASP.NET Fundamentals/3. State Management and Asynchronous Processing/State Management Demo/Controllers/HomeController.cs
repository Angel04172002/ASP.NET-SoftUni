using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StateManagement.Models;
using System.Diagnostics;
using System.Text;

namespace StateManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
      

            _logger = logger;
        }

        public IActionResult Index()
        {
            this.HttpContext.Session.Set("Username", Encoding.ASCII.GetBytes("Pesho"));

            this.HttpContext.Session.Set("Product1Name", Encoding.ASCII.GetBytes("Milk"));
            this.HttpContext.Session.Set("Product2Name", Encoding.ASCII.GetBytes("Cookies"));

            return View();
        }


        public IActionResult BuyProduct()
        {
            var cart = new
            {
                CardId = Guid.NewGuid().ToString(),
                Items = new[]
                {
                    new
                    {
                        Name= "Milk",
                        Price = 3.90
                    },
                    new
                    {
                        Name= "Cookies",
                        Price = 10.90
                    },
                }
            };

            this.HttpContext.Session.SetString("cart", JsonConvert.ToString(cart));

            return View();
        }

  

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
