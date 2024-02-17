using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data;
using SoftUniBazar.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SoftUniBazar.Controllers
{
    [Authorize]
    public class AdController : Controller
    {
        private readonly BazarDbContext context;

        public AdController(BazarDbContext _context)
        {
            context = _context;
        }


        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await context.Ads
                .AsNoTracking()
                .Select(a => new AdInfoViewModel(
                        a.Id,
                        a.Name,
                        a.Description,
                        a.Price,
                        a.Owner.UserName,
                        a.ImageUrl,
                        a.CreatedOn,
                        a.Category.Name
                 ))
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            
            var ad = await context.Ads
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if(ad == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            var adBuyer = new AdBuyer()
            {
                AdId = ad.Id,
                BuyerId = userId
            };


            if(!await context.AdBuyers.ContainsAsync(adBuyer))
            {
                await context.AdBuyers.AddAsync(adBuyer);
                await context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Cart));

        }


        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            string userId = GetUserId();

            var ads = await context.AdBuyers
                .Where(x => x.BuyerId == userId)
                .Select(x => new AdInfoViewModel(
                    x.AdId,
                    x.Ad.Name,
                    x.Ad.Description,
                    x.Ad.Price,
                    x.Ad.Owner.UserName,
                    x.Ad.ImageUrl,
                    x.Ad.CreatedOn,
                    x.Ad.Category.Name
                 ))
                .ToListAsync();

            return View(ads);

        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var ad = await context.Ads
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if(ad == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            var buyer = await context.AdBuyers
                .Where(x => x.BuyerId == userId && x.AdId == ad.Id)
                .FirstOrDefaultAsync();

            if(buyer == null)
            {
                return BadRequest();
            }

            context.AdBuyers.Remove(buyer);

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(All));

        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new FormViewModel();

            model.Categories = await GetCategoriesAsync();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(FormViewModel model)
        {
            var categories = await GetCategoriesAsync();

            if(!categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist!");
            }

            if(!ModelState.IsValid)
            {
                model.Categories = await GetCategoriesAsync();

                return View(model);
            }

            string userId = GetUserId();

            Ad ad = new Ad()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                CategoryId = model.CategoryId,
                OwnerId = userId,
                CreatedOn = DateTime.Now
            };

            await context.Ads.AddAsync(ad);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ad = await context.Ads
                .FindAsync(id);

            if(ad == null) 
            {
                return BadRequest();
            }

            var model = new FormViewModel()
            {
                Name = ad.Name,
                Description = ad.Description,
                Price = ad.Price,
                ImageUrl = ad.ImageUrl,
                CategoryId = ad.CategoryId
            };

            model.Categories = await GetCategoriesAsync();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(FormViewModel model, int id)
        {
            var categories = await GetCategoriesAsync();

            if(!categories.Any(x => x.Id == model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist!");
            }

            if(!ModelState.IsValid)
            {
                model.Categories = await GetCategoriesAsync();

                return View(model);
            }

            var ad = await context.Ads
                .FindAsync(id);


            if (ad == null)
            {
                return BadRequest();
            }


            ad.Name = model.Name;
            ad.Description = model.Description;
            ad.Price = model.Price;
            ad.ImageUrl = model.ImageUrl;
            ad.CategoryId = model.CategoryId;

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        private async Task<List<CategoryViewModel>> GetCategoriesAsync()
        {
            return await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }


    }
}
