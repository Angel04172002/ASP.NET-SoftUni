using Microsoft.EntityFrameworkCore;
using TaskBoardApp.Data;
using TaskBoardApp.Models;
using TaskBoardApp.Services.Contracts;


namespace TaskBoardApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskBoardAppDbContext context;

        public TaskService(TaskBoardAppDbContext _context)
        {
            context = _context;
        }

        public async Task CreateTaskAsync(string ownerId, TaskFormModel model)
        {
            Data.Models.Task task = new Data.Models.Task() 
            {
                Title = model.Title,
                Description = model.Description,
                CreatedOn = DateTime.Now,
                OwnerId = ownerId,
                BoardId = model.BoardId
            };

            await context.AddAsync(task);
            await context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(Data.Models.Task task)
        {
            context.Remove(task);
            await context.SaveChangesAsync();
        }

        public async Task EditTaskAsync(int id, TaskFormModel model)
        {
            var task = await context.Tasks
                  .Where(t => t.Id == id)
                  .FirstOrDefaultAsync();

            task.Title = model.Title;
            task.Description = model.Description;
            task.BoardId = model.BoardId;

            await context.SaveChangesAsync();

        }

        public async Task<Data.Models.Task> FindTaskAsync(int id)
        {
            var task = await context.Tasks
                .FindAsync(id);

            return task;
        }

        public async Task<TaskDetailsViewModel> GetTaskDetailsAsync(int id)
        {
            var task = await context.Tasks
                .AsNoTracking()
                .Select(t => new TaskDetailsViewModel()
                {
                    Id = id,
                    Title = t.Title,
                    Description = t.Description,
                    CreatedOn = t.CreatedOn.ToString(),
                    Owner = t.Owner.UserName
                })
                .FirstOrDefaultAsync(t => t.Id == id);

            return task;
        }
    }
}
