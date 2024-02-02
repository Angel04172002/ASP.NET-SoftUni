using Microsoft.AspNetCore.Mvc;

namespace MvcIntroDemo.Controllers
{
    public class NumbersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Limit(int num)
        {
            return View(num);
        }
    }
}
