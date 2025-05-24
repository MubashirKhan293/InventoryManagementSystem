using InventoryManagement.Server.Context;
using InventoryManagement.Server.DTOs;
using InventoryManagement.Server.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Server.Services.SaleService
{
    public class SaleService : ISaleService
    {
        private readonly ApplicationDbContext _context;

        public SaleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SaleDto>> GetAllSalesAsync()
        {
            var sales = await _context.Sales
                .Include(s => s.Product)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();

            return sales.Select(s => new SaleDto
            {
                Id = s.Id,
                ProductId = s.ProductId,
                ProductName = s.Product.Name,
                QuantitySold = s.QuantitySold,
                UnitPrice = s.UnitPrice,
                TotalAmount = s.TotalAmount,
                SaleDate = s.SaleDate,
                CustomerName = s.CustomerName
            });
        }

        public async Task<SaleDto?> GetSaleByIdAsync(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.Product)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null) return null;

            return new SaleDto
            {
                Id = sale.Id,
                ProductId = sale.ProductId,
                ProductName = sale.Product.Name,
                QuantitySold = sale.QuantitySold,
                UnitPrice = sale.UnitPrice,
                TotalAmount = sale.TotalAmount,
                SaleDate = sale.SaleDate,
                CustomerName = sale.CustomerName
            };
        }

        public async Task<SaleDto?> CreateSaleAsync(CreateSaleDto createSaleDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Check if product exists and has enough quantity
                var product = await _context.Products.FindAsync(createSaleDto.ProductId);
                if (product == null)
                    throw new ArgumentException("Product not found");

                if (product.Quantity < createSaleDto.QuantitySold)
                    throw new InvalidOperationException("Insufficient product quantity");

                // Create sale record
                var sale = new Sale
                {
                    ProductId = createSaleDto.ProductId,
                    QuantitySold = createSaleDto.QuantitySold,
                    UnitPrice = createSaleDto.UnitPrice,
                    CustomerName = createSaleDto.CustomerName,
                    SaleDate = DateTime.UtcNow
                };

                _context.Sales.Add(sale);

                // Update product quantity
                product.Quantity -= createSaleDto.QuantitySold;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Return the created sale
                await _context.Entry(sale).Reference(s => s.Product).LoadAsync();

                return new SaleDto
                {
                    Id = sale.Id,
                    ProductId = sale.ProductId,
                    ProductName = sale.Product.Name,
                    QuantitySold = sale.QuantitySold,
                    UnitPrice = sale.UnitPrice,
                    TotalAmount = sale.TotalAmount,
                    SaleDate = sale.SaleDate,
                    CustomerName = sale.CustomerName
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<SaleDto>> GetSalesByProductIdAsync(int productId)
        {
            var sales = await _context.Sales
                .Include(s => s.Product)
                .Where(s => s.ProductId == productId)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();

            return sales.Select(s => new SaleDto
            {
                Id = s.Id,
                ProductId = s.ProductId,
                ProductName = s.Product.Name,
                QuantitySold = s.QuantitySold,
                UnitPrice = s.UnitPrice,
                TotalAmount = s.TotalAmount,
                SaleDate = s.SaleDate,
                CustomerName = s.CustomerName
            });
        }
    }
}
