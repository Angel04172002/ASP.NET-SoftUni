using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TaskBoardApp.Data;
using TaskBoardApp.Models;
using TaskBoardApp.Services.Contracts;

namespace TaskBoardApp.Services
{
    public class BoardService : IBoardService
    {
        private readonly TaskBoardAppDbContext context;

        public BoardService(TaskBoardAppDbContext _context)
        {
            context = _context;   
        }

        public async Task<IEnumerable<BoardAllViewModel>> GetAllBoardsAsync()
        {
            var boards = await context.Boards
                .AsNoTracking()
                .Select(b => new BoardAllViewModel()
                {
                    Id = b.Id,
                    Name = b.Name,
                    Tasks = b.Tasks.Select(t => new TaskViewModel()
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        Owner = t.Owner.UserName
                    })
                    .ToList()
                })
                .ToListAsync();

            return boards;
        }



        public async Task<IEnumerable<BoardSelectViewModel>> GetCategories()
        {
            return await context.Boards
                .AsNoTracking()
                .Select(b => new BoardSelectViewModel()
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToListAsync();
        }
    }
}
