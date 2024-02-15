using ForumApp.Core.Contracts;
using ForumApp.Core.Models;
using ForumApp.Infrastructure.Data;
using ForumApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Core.Services
{
    public class PostService : IPostService
    {
        private readonly ForumAppDbContext context;

        public PostService(ForumAppDbContext _context)
        {
            context = _context;
        }

        public async Task AddPostAsync(PostModel model)
        {
            Post post = new Post()
            {
                Content = model.Content,
                Title = model.Title
            };

            await context.AddAsync(post);
            await context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int id)
        {
            Post post = await GetByIdAsync(id);

            context.Remove(post);
            await context.SaveChangesAsync();   
        }

        public async Task EditPostAsync(PostModel model)
        {
            Post post = await GetByIdAsync(model.Id);

            post.Title = model.Title;
            post.Content = model.Content;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PostModel>> GetAllPostsAsync()
        {
            return await context.Posts
                 .AsNoTracking()
                 .Select(p => new PostModel()
                 {
                     Id = p.Id,
                     Title = p.Title,
                     Content = p.Content
                 })
                 .ToListAsync();
        }

        public async Task<PostModel?> GetPostByIdAsync(int id)
        {
            return await context.Posts
                .Where(p => p.Id == id)
                .Select(p => new PostModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content
                })
                .FirstOrDefaultAsync();
        }

        private async Task<Post> GetByIdAsync(int id)
        {
            Post post = await context.FindAsync<Post>(id);

            if (post == null)
            {
                throw new ApplicationException("Invalid Post");
            }

            return post;
        }
    }
}
