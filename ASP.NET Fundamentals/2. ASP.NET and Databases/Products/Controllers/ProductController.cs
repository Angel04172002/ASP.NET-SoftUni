using ASP.NET_And_Databases.Contracts;
using ASP.NET_And_Databases.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_And_Databases.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await productService.GetAllProductsAsync();

            return View(model);
        }


        [HttpGet]
        public IActionResult Add()
        {
            var model = new ProductViewModel();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(ProductViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            await productService.AddProductAsync(model);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await productService.GetProductByIdAsync(id);

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model, int id)
        {
            if(!ModelState.IsValid || model.Id != id)
            {
                return View(model);
            }

            await productService.UpdateProductAsync(model);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await productService.DeleteProductAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
