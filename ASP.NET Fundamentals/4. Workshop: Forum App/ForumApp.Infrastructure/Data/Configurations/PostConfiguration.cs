using ForumApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumApp.Infrastructure.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        private IEnumerable<Post> posts = new List<Post>()
        {
            new Post() { Id=1, Title= "My First Post", Content = "My first post content"},
            new Post() { Id=2, Title= "My Second Post", Content = "My second post content"},
            new Post() { Id=3, Title= "My Third Post", Content = "My third post content"},
        };

        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasData(posts);
        }
    }
}
