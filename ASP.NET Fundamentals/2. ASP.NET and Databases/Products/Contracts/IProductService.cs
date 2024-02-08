using ASP.NET_And_Databases.Models;

namespace ASP.NET_And_Databases.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();

        Task<ProductViewModel> GetProductByIdAsync(int id);

        Task AddProductAsync(ProductViewModel model);

        Task UpdateProductAsync(ProductViewModel model);

        Task DeleteProductAsync(int id);

    }
}
