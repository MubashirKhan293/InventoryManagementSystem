using InventoryManagement.Server.DTOs;

namespace InventoryManagement.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(ProductInputDto createProductDto);
        Task<ProductDto?> UpdateProductAsync(int id, ProductInputDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id);
    }
}
