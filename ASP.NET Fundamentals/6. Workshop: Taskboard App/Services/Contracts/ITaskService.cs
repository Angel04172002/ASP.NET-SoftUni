using TaskBoardApp.Models;

namespace TaskBoardApp.Services.Contracts
{
    public interface ITaskService
    {
        Task CreateTaskAsync(string ownerId, TaskFormModel model);

        Task<TaskDetailsViewModel> GetTaskDetailsAsync(int id);

        Task EditTaskAsync(int id, TaskFormModel model);

        Task<Data.Models.Task> FindTaskAsync(int id);

        Task DeleteTaskAsync(Data.Models.Task task);
    }
}
