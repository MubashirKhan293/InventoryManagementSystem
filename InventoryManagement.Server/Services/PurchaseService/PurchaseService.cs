using InventoryManagement.Server.Context;
using InventoryManagement.Server.DTOs;
using InventoryManagement.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Server.Services.PurchaseService
{
    public class PurchaseService : IPurchaseService
    {
        private readonly ApplicationDbContext _context;

        public PurchaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseDto>> GetAllPurchasesAsync()
        {
            return await _context.Purchases
                .Include(p => p.Items)
                .ThenInclude(pi => pi.Product)
                .Select(p => new PurchaseDto
                {
                    Id = p.Id,
                    PurchaseDate = p.PurchaseDate,
                    SupplierName = p.SupplierName,
                    TotalAmount = p.TotalAmount,
                    Notes = p.Notes,
                    Status = p.Status,
                    Items = p.Items.Select(pi => new PurchaseItemDto
                    {
                        Id = pi.Id,
                        ProductId = pi.ProductId,
                        ProductName = pi.Product.Name,
                        Quantity = pi.Quantity,
                        UnitPrice = pi.UnitPrice,
                        TotalPrice = pi.TotalPrice
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<PurchaseDto> GetPurchaseByIdAsync(int id)
        {
            var purchase = await _context.Purchases
                .Include(p => p.Items)
                .ThenInclude(pi => pi.Product)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null)
                return null;

            return new PurchaseDto
            {
                Id = purchase.Id,
                PurchaseDate = purchase.PurchaseDate,
                SupplierName = purchase.SupplierName,
                TotalAmount = purchase.TotalAmount,
                Notes = purchase.Notes,
                Status = purchase.Status,
                Items = purchase.Items.Select(pi => new PurchaseItemDto
                {
                    Id = pi.Id,
                    ProductId = pi.ProductId,
                    ProductName = pi.Product.Name,
                    Quantity = pi.Quantity,
                    UnitPrice = pi.UnitPrice,
                    TotalPrice = pi.TotalPrice
                }).ToList()
            };
        }

        public async Task<PurchaseDto> CreatePurchaseAsync(CreatePurchaseDto purchaseDto)
        {
            // Calculate total amount
            decimal totalAmount = 0;
            var purchaseItems = new List<PurchaseItem>();

            foreach (var item in purchaseDto.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product with ID {item.ProductId} not found.");

                decimal itemTotal = item.UnitPrice * item.Quantity;
                totalAmount += itemTotal;

                // Create purchase item
                var purchaseItem = new PurchaseItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = itemTotal
                };

                purchaseItems.Add(purchaseItem);
            }

            // Create purchase
            var purchase = new Purchase
            {
                PurchaseDate = DateTime.UtcNow,
                SupplierName = purchaseDto.SupplierName,
                TotalAmount = totalAmount,
                Notes = purchaseDto.Notes,
                Status = "Pending", // Initial status
                CreatedAt = DateTime.UtcNow,
                Items = purchaseItems
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            // Return the created purchase
            return await GetPurchaseByIdAsync(purchase.Id);
        }

        public async Task<PurchaseDto> UpdatePurchaseStatusAsync(int id, UpdatePurchaseStatusDto statusDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var purchase = await _context.Purchases
                    .Include(p => p.Items)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (purchase == null)
                    return null;

                // If changing to "Received", update product quantities
                if (statusDto.Status == "Received" && purchase.Status != "Received")
                {
                    foreach (var item in purchase.Items)
                    {
                        var product = await _context.Products.FindAsync(item.ProductId);
                        if (product == null)
                            throw new Exception($"Product with ID {item.ProductId} not found.");

                        // Update product quantity
                        product.Quantity += item.Quantity;
                        product.UpdatedAt = DateTime.UtcNow;
                    }
                }
                // If changing from "Received" to something else, reverse the quantity updates
                else if (purchase.Status == "Received" && statusDto.Status != "Received")
                {
                    foreach (var item in purchase.Items)
                    {
                        var product = await _context.Products.FindAsync(item.ProductId);
                        if (product == null)
                            throw new Exception($"Product with ID {item.ProductId} not found.");

                        // Make sure we don't end up with negative inventory
                        if (product.Quantity < item.Quantity)
                            throw new Exception($"Cannot change status: Product {product.Name} has insufficient inventory.");

                        // Reverse quantity update
                        product.Quantity -= item.Quantity;
                        product.UpdatedAt = DateTime.UtcNow;
                    }
                }

                purchase.Status = statusDto.Status;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Return the updated purchase
                return await GetPurchaseByIdAsync(purchase.Id);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
