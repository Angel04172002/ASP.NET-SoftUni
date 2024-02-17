using Homies.Data;
using Homies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace Homies.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly HomiesDbContext context;

        public EventController(HomiesDbContext _context)
        {
            context = _context; 
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var events = await context.Events
                .AsNoTracking()
                .Select(e => new EventInfoViewModel(
                    e.Id,
                    e.Name,
                    e.Start,
                    e.Organiser.UserName,
                    e.Type.Name))
                .ToListAsync();

            return View(events);

        }



        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            var e = await context.Events
                .Where(x => x.Id == id)
                .Include(e => e.EventsParticipants)
                .FirstOrDefaultAsync();

            if(e == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if(!e.EventsParticipants.Any(p => p.HelperId == userId))
            {
                e.EventsParticipants.Add(new EventParticipant()
                {
                    HelperId = userId,
                    EventId = e.Id
                });

                await context.SaveChangesAsync();
            }

           
            return RedirectToAction(nameof(Joined));

        }



        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            string userId = GetUserId();

            var events = await context.EventParticipants
                .AsNoTracking()
                .Where(ep => ep.HelperId == userId)
                .Select(e => new EventInfoViewModel(
                    e.EventId,
                    e.Event.Name,
                    e.Event.Start,
                    e.Event.Organiser.UserName,
                    e.Event.Type.Name))
                .ToListAsync();

            return View(events);
        }


        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var e = await context.Events
                .Where(x => x.Id == id)
                .Include(e => e.EventsParticipants)
                .FirstOrDefaultAsync();


            if (e == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            var ep = e.EventsParticipants
                   .FirstOrDefault(ep => ep.HelperId == userId);

            if(ep == null)
            {
                return BadRequest();
            }

            e.EventsParticipants.Remove(ep);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new EventFormViewModel();
            model.Types = await GetTypes();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(EventFormViewModel model)
        {
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            if(!DateTime.TryParseExact(
                model.Start,
                DataConstants.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out start))
            {
                ModelState
                    .AddModelError(nameof(model.Start), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            if (!DateTime.TryParseExact(
             model.End,
             DataConstants.DateFormat,
             CultureInfo.InvariantCulture,
             DateTimeStyles.None,
             out end))
            {
                ModelState
                    .AddModelError(nameof(model.End), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            if(!ModelState.IsValid)
            {
                model.Types = await GetTypes();

                return View(model);
            }

            var ev = new Event()
            {
                CreatedOn = DateTime.Now,
                Description = model.Description,
                Name = model.Name,
                OrganiserId = GetUserId(),
                TypeId = model.TypeId,
                Start = start,
                End = end
            };

            await context.Events.AddAsync(ev);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var e = await context.Events
                .FindAsync(id);

            if(e == null)
            {
                return BadRequest();
            }

            if(e.OrganiserId != GetUserId())
            {
                return Unauthorized();
            }

            var model = new EventFormViewModel()
            {
                Description = e.Description,
                Name = e.Name,
                Start = e.Start.ToString(DataConstants.DateFormat),
                End = e.End.ToString(DataConstants.DateFormat),
                TypeId = e.TypeId
            };

            model.Types = await GetTypes();

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(EventFormViewModel model, int id)
        {
            var e = await context.Events
                .FindAsync(id);

            if (e == null)
            {
                return BadRequest();
            }

            if (e.OrganiserId != GetUserId())
            {
                return Unauthorized();
            }


            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;

            if (!DateTime.TryParseExact(
                model.Start,
                DataConstants.DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out start))
            {
                ModelState
                    .AddModelError(nameof(model.Start), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            if (!DateTime.TryParseExact(
             model.End,
             DataConstants.DateFormat,
             CultureInfo.InvariantCulture,
             DateTimeStyles.None,
             out end))
            {
                ModelState
                   .AddModelError(nameof(model.End), $"Invalid date! Format must be: {DataConstants.DateFormat}");
            }

            if(!ModelState.IsValid)
            {
                model.Types = await GetTypes();
                return View(model);
            }

            e.Start = start;
            e.End = end;
            e.Description = model.Description;
            e.Name = model.Name;
            e.TypeId = model.TypeId;

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await context.Events
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(ep => new EventDetailsViewModel()
                {
                    Id = ep.Id,
                    CreatedOn = ep.CreatedOn.ToString(DataConstants.DateFormat),
                    Description = ep.Description,
                    End = ep.End.ToString(DataConstants.DateFormat),
                    Start = ep.Start.ToString(DataConstants.DateFormat),
                    Name = ep.Name,
                    Organiser = ep.Organiser.UserName,
                    Type = ep.Type.Name
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);


        }

        private async Task<IEnumerable<TypeViewModel>> GetTypes()
        {
            return await context.Types
                .AsNoTracking()
                .Select(t => new TypeViewModel()
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }
    }
}
