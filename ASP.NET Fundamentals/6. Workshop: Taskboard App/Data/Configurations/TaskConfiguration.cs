using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskBoardApp.Data.Models;
using Task = TaskBoardApp.Data.Models.Task;

namespace TaskBoardApp.Data.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder
                .HasOne(b => b.Board)
                .WithMany(t => t.Tasks)
                .HasForeignKey(t => t.BoardId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(SeedTasks());
        }

        private List<Task> SeedTasks()
        {
            List<Task> tasks = new List<Task>();

            Task currentTask = new Task()
            {
                Id = 1,
                Title = "Improve CSS Styles",
                Description = "Implement better styling for all public pages",
                CreatedOn = DateTime.UtcNow.AddDays(-200),
                OwnerId = "71dd3886-d9fc-4936-9a0b-47b14cbc84f1",
                BoardId = 1
            };


            tasks.Add(currentTask);

            currentTask = new Task()
            {
                Id = 2,
                Title = "Android Client App",
                Description = "Create Android Client App for the Taskboard Restful API",
                CreatedOn = DateTime.UtcNow.AddDays(-100),
                OwnerId = "71dd3886-d9fc-4936-9a0b-47b14cbc84f1",
                BoardId = 2
            };

            tasks.Add(currentTask);

            currentTask = new Task()
            {
                Id = 3,
                Title = "Desktop Client App",
                Description = "Create Windows forms desktop app client for the Taskboard Restful API",
                CreatedOn = DateTime.UtcNow.AddDays(-50),
                OwnerId = "71dd3886-d9fc-4936-9a0b-47b14cbc84f1",
                BoardId = 3
            };

            tasks.Add(currentTask);

            return tasks;
        }
    }
}
