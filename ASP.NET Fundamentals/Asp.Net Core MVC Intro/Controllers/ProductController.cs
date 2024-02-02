using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MvcIntroDemo.Models.Product;
using System.Text;
using System.Text.Json;

namespace MvcIntroDemo.Controllers
{
    public class ProductController : Controller
    {
        private IEnumerable<ProductViewModel> products 
         = new List<ProductViewModel>()
            {
                new ProductViewModel()
                {
                    Id=1,
                    Name="Cheese",
                    Price=7.00
                },
                new ProductViewModel()
                {
                    Id=2,
                    Name="Ham",
                    Price=5.50
                },
                new ProductViewModel()
                {
                    Id=3,
                    Name="Bread",
                    Price=1.50
                }
            };



        public IActionResult Index()
        {
            return View();
        }


        public IActionResult All(string keyword)
        {
            if(keyword == null)
            {
                return View(products);
            }

            var filteredProducts = products
                .Where(x => x.Name.ToLower().Contains(keyword.ToLower()))
                .ToList();

            return View(filteredProducts);
        }

        public IActionResult ById(int id)
        {
            ProductViewModel product = products.FirstOrDefault(p => p.Id == id);

            if(product == null)
            {
                ViewBag.Error = "Product Not Found!";

                return RedirectToAction(nameof(All));
            }


            return View(product);
        }


        public IActionResult AllAsJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            return Json(products, options);
        }


        public IActionResult AllAsText()
        {
            return Content(GetAllAsString());
        }


        public IActionResult AllAsTextFile()
        {
            var productsInfo = GetAllAsString();

            Response.Headers
                .Add(HeaderNames.ContentDisposition, @"attachment;filename=products.txt");

            return File(Encoding.UTF8.GetBytes(productsInfo), "text/plain");
        }


        private string GetAllAsString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in products)
            {
                sb.AppendLine($"Product {item.Id}: {item.Name} - {item.Price} lv.");
            }

            return sb.ToString().TrimEnd();
        }

    }
}
