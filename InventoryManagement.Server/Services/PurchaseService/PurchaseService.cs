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

        public async Task<IEnumerable<PurchaseDto>> GetAllPurchasesAsync()
        {
            var purchases = await _context.Purchases
                .Include(p => p.Product)
                .OrderByDescending(p => p.PurchaseDate)
                .ToListAsync();

            return purchases.Select(p => new PurchaseDto
            {
                Id = p.Id,
                ProductId = p.ProductId,
                ProductName = p.Product.Name,
                QuantityPurchased = p.QuantityPurchased,
                UnitPrice = p.UnitPrice,
                TotalAmount = p.TotalAmount,
                PurchaseDate = p.PurchaseDate,
                SupplierName = p.SupplierName
            });
        }

        public async Task<PurchaseDto?> GetPurchaseByIdAsync(int id)
        {
            var purchase = await _context.Purchases
                .Include(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null) return null;

            return new PurchaseDto
            {
                Id = purchase.Id,
                ProductId = purchase.ProductId,
                ProductName = purchase.Product.Name,
                QuantityPurchased = purchase.QuantityPurchased,
                UnitPrice = purchase.UnitPrice,
                TotalAmount = purchase.TotalAmount,
                PurchaseDate = purchase.PurchaseDate,
                SupplierName = purchase.SupplierName
            };
        }

        public async Task<PurchaseDto?> CreatePurchaseAsync(CreatePurchaseDto createPurchaseDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Check if product exists
                var product = await _context.Products.FindAsync(createPurchaseDto.ProductId);
                if (product == null)
                    throw new ArgumentException("Product not found");

                // Create purchase record
                var purchase = new Purchase
                {
                    ProductId = createPurchaseDto.ProductId,
                    QuantityPurchased = createPurchaseDto.QuantityPurchased,
                    UnitPrice = createPurchaseDto.UnitPrice,
                    SupplierName = createPurchaseDto.SupplierName,
                    PurchaseDate = DateTime.UtcNow
                };

                _context.Purchases.Add(purchase);

                // Update product quantity
                product.Quantity += createPurchaseDto.QuantityPurchased;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Return the created purchase
                await _context.Entry(purchase).Reference(p => p.Product).LoadAsync();

                return new PurchaseDto
                {
                    Id = purchase.Id,
                    ProductId = purchase.ProductId,
                    ProductName = purchase.Product.Name,
                    QuantityPurchased = purchase.QuantityPurchased,
                    UnitPrice = purchase.UnitPrice,
                    TotalAmount = purchase.TotalAmount,
                    PurchaseDate = purchase.PurchaseDate,
                    SupplierName = purchase.SupplierName
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<PurchaseDto>> GetPurchasesByProductIdAsync(int productId)
        {
            var purchases = await _context.Purchases
                .Include(p => p.Product)
                .Where(p => p.ProductId == productId)
                .OrderByDescending(p => p.PurchaseDate)
                .ToListAsync();

            return purchases.Select(p => new PurchaseDto
            {
                Id = p.Id,
                ProductId = p.ProductId,
                ProductName = p.Product.Name,
                QuantityPurchased = p.QuantityPurchased,
                UnitPrice = p.UnitPrice,
                TotalAmount = p.TotalAmount,
                PurchaseDate = p.PurchaseDate,
                SupplierName = p.SupplierName
            });
        }
    }
}
