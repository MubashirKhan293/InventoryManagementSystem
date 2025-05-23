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

        public async Task<List<SaleDto>> GetAllSalesAsync()
        {
            return await _context.Sales
                .Include(s => s.Items)
                .ThenInclude(si => si.Product)
                .Select(s => new SaleDto
                {
                    Id = s.Id,
                    SaleDate = s.SaleDate,
                    CustomerName = s.CustomerName,
                    TotalAmount = s.TotalAmount,
                    Notes = s.Notes,
                    Items = s.Items.Select(si => new SaleItemDto
                    {
                        Id = si.Id,
                        ProductId = si.ProductId,
                        ProductName = si.Product.Name,
                        Quantity = si.Quantity,
                        UnitPrice = si.UnitPrice,
                        TotalPrice = si.TotalPrice
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<SaleDto> GetSaleByIdAsync(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.Items)
                .ThenInclude(si => si.Product)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
                return null;

            return new SaleDto
            {
                Id = sale.Id,
                SaleDate = sale.SaleDate,
                CustomerName = sale.CustomerName,
                TotalAmount = sale.TotalAmount,
                Notes = sale.Notes,
                Items = sale.Items.Select(si => new SaleItemDto
                {
                    Id = si.Id,
                    ProductId = si.ProductId,
                    ProductName = si.Product.Name,
                    Quantity = si.Quantity,
                    UnitPrice = si.UnitPrice,
                    TotalPrice = si.TotalPrice
                }).ToList()
            };
        }

        public async Task<SaleDto> CreateSaleAsync(CreateSaleDto saleDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Calculate total amount and validate product quantities
                decimal totalAmount = 0;
                var saleItems = new List<SaleItem>();

                foreach (var item in saleDto.Items)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null)
                        throw new Exception($"Product with ID {item.ProductId} not found.");

                    if (product.Quantity < item.Quantity)
                        throw new Exception($"Insufficient quantity for product: {product.Name}. Available: {product.Quantity}, Requested: {item.Quantity}");

                    decimal itemTotal = product.UnitPrice * item.Quantity;
                    totalAmount += itemTotal;

                    // Update product quantity
                    product.Quantity -= item.Quantity;
                    product.UpdatedAt = DateTime.UtcNow;

                    // Create sale item
                    var saleItem = new SaleItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.UnitPrice,
                        TotalPrice = itemTotal
                    };

                    saleItems.Add(saleItem);
                }

                // Create sale
                var sale = new Sale
                {
                    SaleDate = DateTime.UtcNow,
                    CustomerName = saleDto.CustomerName,
                    TotalAmount = totalAmount,
                    Notes = saleDto.Notes,
                    CreatedAt = DateTime.UtcNow,
                    Items = saleItems
                };

                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // Map to DTO
                var result = new SaleDto
                {
                    Id = sale.Id,
                    SaleDate = sale.SaleDate,
                    CustomerName = sale.CustomerName,
                    TotalAmount = sale.TotalAmount,
                    Notes = sale.Notes,
                    Items = sale.Items.Select(si => new SaleItemDto
                    {
                        Id = si.Id,
                        ProductId = si.ProductId,
                        ProductName = si.Product.Name,
                        Quantity = si.Quantity,
                        UnitPrice = si.UnitPrice,
                        TotalPrice = si.TotalPrice
                    }).ToList()
                };

                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
