using ASP.NET_And_Databases.Contracts;
using ASP.NET_And_Databases.Data;
using ASP.NET_And_Databases.Data.Models;
using ASP.NET_And_Databases.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_And_Databases.Services
{
    public class ProductService : IProductService
    {
        private readonly ShoppingListDbContext context;

        public ProductService(ShoppingListDbContext _context)
        {
            context = _context;
        }

        public async Task AddProductAsync(ProductViewModel model)
        {
            var entity = new Product()
            {
                Name = model.Name
            };

            await context.Products.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var entity = await context.Products.FindAsync(id);

            if(entity == null)
            {
                throw new ArgumentException("Invalid product");
            }

            context.Products.Remove(entity);

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
           return await context.Products
                .AsNoTracking()
                .Select(p => new ProductViewModel()
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            var entity = await context.Products.FindAsync(id);

            if(entity == null)
            {
                throw new ArgumentException("Invalid product");
            }

            return new ProductViewModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public async Task UpdateProductAsync(ProductViewModel model)
        {
            var entity = await context.Products.FindAsync(model.Id);

            if (entity == null)
            {
                throw new ArgumentException("Invalid product");
            }

            entity.Name = model.Name;

            await context.SaveChangesAsync();
        }
    }
}
