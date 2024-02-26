using AspNetCoreAdvancedDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Diagnostics;

namespace AspNetCoreAdvancedDemo.Controllers
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
            return View();
        }



        [HttpGet]
        public IActionResult UploadFiles() => View();

    
        [HttpPost]
        public async Task<IActionResult> UploadFiles(IEnumerable<IFormFile> files)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Files");


            foreach (var file in files.Where(f => f.Length > 0))
            {
                string filename = Path.Combine(path, file.FileName);

                using(var fileStream = new FileStream(filename, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }


            return Ok(
                new 
                {
                    savedFilesLength = files.Sum(f => f.Length) 
                });

        }


        public IActionResult Download(string filename)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Files");

            IFileProvider fileProvider = new PhysicalFileProvider(path);
            IFileInfo fileInfo = fileProvider.GetFileInfo(filename);

            var stream = fileInfo.CreateReadStream();
            var mimeType = "application/octet-stream";

            return File(stream, mimeType, filename);
        }


        public IActionResult GetPrice(decimal price)
        {
            return Ok(new
            {
                price
            });
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
