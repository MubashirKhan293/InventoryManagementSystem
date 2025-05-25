using InventoryManagement.Server.Context;
using InventoryManagement.Server.DTOs;
using InventoryManagement.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Quantity = p.Quantity,
                CreatedDate = p.CreatedDate
            });
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                CreatedDate = product.CreatedDate
            };
        }

        public async Task<ProductDto> CreateProductAsync(ProductInputDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Quantity = createProductDto.Quantity,
                CreatedDate = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                CreatedDate = product.CreatedDate
            };
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, ProductInputDto updateProductDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Quantity = updateProductDto.Quantity;

            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                CreatedDate = product.CreatedDate
            };
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return false;
                }

                // Delete related sales first
                var sales = await _context.Sales.Where(s => s.ProductId == id).ToListAsync();
                if (sales.Any())
                {
                    _context.Sales.RemoveRange(sales);
                }

                // Delete related purchases
                var purchases = await _context.Purchases.Where(p => p.ProductId == id).ToListAsync();
                if (purchases.Any())
                {
                    _context.Purchases.RemoveRange(purchases);
                }

                // Finally delete the product
                _context.Products.Remove(product);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> ProductNameExistsAsync(string name)
        {
            var productExists = await _context.Products
                          .AnyAsync(e => e.Name.ToLower() == name.ToLower());

            return productExists;
        }
    }
}
