using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data;
using SeminarHub.Models;
using System.Globalization;
using System.Security.Claims;
using static SeminarHub.Data.DataConstants;

namespace SeminarHub.Controllers
{
    [Authorize]
    public class SeminarController : Controller
    {
        private readonly SeminarHubDbContext context;

        public SeminarController(SeminarHubDbContext _context)
        {
            context = _context;
        }


        [HttpGet]
        public async Task<IActionResult> All()
        {
            var seminars = await context.Seminars
                .AsNoTracking()
                .Select(s => new SeminarInfoViewModel(
                    s.Id,
                    s.Topic,
                    s.Lecturer,
                    s.Category.Name,
                    s.DateAndTime.ToString(DateFormat),
                    s.Organizer.UserName
           
                ))
                .ToListAsync();

            return View(seminars);
        }


        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
           
            var seminar = await context.Seminars
                .Where(s => s.Id == id)
                .Include(s => s.SeminarsParticipants)
                .FirstOrDefaultAsync();

            if(seminar == null) 
            {
                return BadRequest();
            }


            string userId = GetUserId();


            if(!seminar.SeminarsParticipants.Any(x => x.ParticipantId == userId))
            {
                seminar.SeminarsParticipants.Add(new SeminarParticipant()
                {
                    ParticipantId = userId,
                    SeminarId = seminar.Id
                });

                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Joined));
            }

            return RedirectToAction(nameof(All));
        }


        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();

            var seminars = await context.SeminarsParticipants
                .AsNoTracking()
                .Where(sp => sp.ParticipantId == userId)
                .Select(s => new SeminarInfoViewModel(
                    s.SeminarId,
                    s.Seminar.Topic,
                    s.Seminar.Lecturer,
                    s.Seminar.Category.Name,
                    s.Seminar.DateAndTime.ToString(DateFormat),
                    s.Seminar.Organizer.UserName
                 ))
                .ToListAsync();

            return View(seminars);
        }


        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var seminar = await context.Seminars
               .Where(s => s.Id == id)
               .Include(s => s.SeminarsParticipants)
               .FirstOrDefaultAsync();

            if (seminar == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            var user = seminar.SeminarsParticipants
                .Where(u => u.ParticipantId == userId)
                .FirstOrDefault();

            if (user == null) 
            {
                return BadRequest();
            }

            seminar.SeminarsParticipants.Remove(user);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Joined));
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new SeminarFormViewModel();
            model.Categories = await GetCategoriesAsync();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(SeminarFormViewModel model)
        {
            DateTime date = ParseDate(model);
     
            var categories = await GetCategoriesAsync();
            model.Categories = categories;

            CheckIfCategoryValid(model);

            if (!ModelState.IsValid)
            {
                model.Categories = categories;
                return View(model);
            }

            string userId = GetUserId();

            var seminar = new Seminar()
            {
                Topic = model.Topic,
                Details = model.Details,
                Duration = model.Duration,
                Lecturer = model.Lecturer,
                OrganizerId = userId,
                DateAndTime = date,
                CategoryId = model.CategoryId
            };


            await context.Seminars.AddAsync(seminar);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(All));

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var seminar = await context.Seminars.FindAsync(id);

            if(seminar == null) 
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (seminar.OrganizerId != userId)
            {
                return Unauthorized();
            }

            var categories = await GetCategoriesAsync();

            var model = new SeminarFormViewModel()
            {
                Topic = seminar.Topic,
                Lecturer = seminar.Lecturer,
                Details = seminar.Details,
                DateAndTime = seminar.DateAndTime.ToString(DateFormat),
                Duration = seminar.Duration,
                CategoryId = seminar.CategoryId,
                Categories = categories
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(SeminarFormViewModel model, int id)
        {
            DateTime date = ParseDate(model);


            var categories = await GetCategoriesAsync();
            model.Categories = categories;

         
            CheckIfCategoryValid(model);

            if (!ModelState.IsValid)
            {
                model.Categories = categories;
                return View(model);
            }


            var seminar = await context.Seminars.FindAsync(id);


            if (seminar == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();


            if (seminar.OrganizerId != userId)
            {
                return Unauthorized();
            }

            seminar.Topic = model.Topic;
            seminar.Lecturer = model.Lecturer;
            seminar.Details = model.Details;
            seminar.CategoryId = model.CategoryId;
            seminar.Duration = model.Duration;
            seminar.DateAndTime = date;

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var seminar = await context.Seminars
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Include(s => s.Category)
                .Include(s => s.Organizer)
                .FirstOrDefaultAsync();

            if(seminar == null)
            {
                return BadRequest();
            }

            var model = new SeminarDetailsViewModel()
            {
                Id = seminar.Id,
                Topic = seminar.Topic,
                DateAndTime = seminar.DateAndTime.ToString(DateFormat),
                Duration = seminar.Duration,
                Lecturer = seminar.Lecturer,
                Details = seminar.Details,
                Category = seminar.Category.Name,
                Organizer = seminar.Organizer.UserName
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await context.Seminars
                .AsNoTracking()
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if(entity == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (entity.OrganizerId != userId)
            {
                return Unauthorized();
            }

            var model = new DeleteFormViewModel()
            {
                Id = entity.Id,
                Topic = entity.Topic,
                DateAndTime = entity.DateAndTime
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await context.Seminars
                  .Where(s => s.Id == id)
                  .FirstOrDefaultAsync();

            if(entity == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (entity.OrganizerId != userId)
            {
                return Unauthorized();
            }

            context.Remove(entity);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }



        private async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
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


        private DateTime ParseDate(SeminarFormViewModel model)
        {
            DateTime date = DateTime.Now;

            if (!DateTime.TryParseExact(
               model.DateAndTime,
               DateFormat,
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out date
               ))
            {
                ModelState.AddModelError(nameof(model.DateAndTime), $"Invalid date! Date format must be {DateFormat}");
                return DateTime.Now;
            }

            return date;
        }

        private void CheckIfCategoryValid(SeminarFormViewModel model)
        {
            if (!model.Categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Invalid category!");
            }
        }
    }
}
