using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskBoardApp.Models;
using TaskBoardApp.Services.Contracts;

namespace TaskBoardApp.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService taskService;
        private readonly IBoardService boardService;

        public TaskController(ITaskService taskService, IBoardService boardService)
        {
            this.taskService = taskService; 
            this.boardService = boardService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new TaskFormModel();
            model.Boards = await boardService.GetCategories();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(TaskFormModel model)
        {
            var boards = await boardService.GetCategories();

            if(!boards.Any(b => b.Id == model.BoardId))
            {
                ModelState.AddModelError(nameof(model.BoardId), "Board does not exist!");
            }


            if (!ModelState.IsValid)
            {
                model.Boards = boards;

                return View(model);
            }

            string userId = GetUserId();

            await taskService.CreateTaskAsync(userId, model);

            return RedirectToAction("All", "Board");
        }


        public async Task<IActionResult> Details(int id)
        {
            var model = await taskService.GetTaskDetailsAsync(id);

            if(model == null)
            {
                return BadRequest();
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await taskService.FindTaskAsync(id);

            if(task == null) 
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if(userId != task.OwnerId)
            {
                return Unauthorized();
            }

            TaskFormModel model = new TaskFormModel()
            {
                Title = task.Title,
                Description = task.Description,
                BoardId = task.BoardId, 
            };

            model.Boards = await boardService.GetCategories();


            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, TaskFormModel model)
        {
            var boards = await boardService.GetCategories();
            var task = await taskService.FindTaskAsync(id);

            if(task == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                model.Boards = boards;
                return View(model);
            }


            string userId = GetUserId();

            if (userId != task.OwnerId)
            {
                return Unauthorized();
            }

            await taskService.EditTaskAsync(id, model);

            return RedirectToAction("All", "Board");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await taskService.FindTaskAsync(id);


            if (task == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (userId != task.OwnerId)
            {
                return Unauthorized();
            }

            var model = new TaskViewModel()
            {
                Id = id,
               Title = task.Title,
               Description = task.Description,

            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(TaskViewModel model)
        {
            var id = model.Id;

            var task = await taskService.FindTaskAsync(id);

            if(task == null)
            {
                return BadRequest();
            }

            string userId = GetUserId();

            if (userId != task.OwnerId)
            {
                return Unauthorized();
            }

            await taskService.DeleteTaskAsync(task);

            return RedirectToAction("All", "Board");
        }


        private string GetUserId()
            => User.FindFirstValue(ClaimTypes.NameIdentifier);

    }
}
