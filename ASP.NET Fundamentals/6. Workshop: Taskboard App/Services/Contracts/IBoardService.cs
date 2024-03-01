using TaskBoardApp.Models;

namespace TaskBoardApp.Services.Contracts
{
    public interface IBoardService
    {
        Task<IEnumerable<BoardAllViewModel>> GetAllBoardsAsync();

        Task<IEnumerable<BoardSelectViewModel>> GetCategories();
    }
}
