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

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice
                })
                .ToListAsync();
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice
            };
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Quantity = productDto.Quantity,
                UnitPrice = productDto.UnitPrice,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice
            };
        }

        public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return null;

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Quantity = productDto.Quantity;
            product.UnitPrice = productDto.UnitPrice;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice
            };
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
