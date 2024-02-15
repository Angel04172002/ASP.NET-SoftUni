
using ForumApp.Core.Models;

namespace ForumApp.Core.Contracts
{
    public interface IPostService
    {
        Task AddPostAsync(PostModel post);
        Task DeletePostAsync(int id);
        Task EditPostAsync(PostModel model);
        Task<IEnumerable<PostModel>> GetAllPostsAsync();
        Task<PostModel?> GetPostByIdAsync(int id);
    }
}
