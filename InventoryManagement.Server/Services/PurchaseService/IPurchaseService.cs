using InventoryManagement.Server.Context;
using InventoryManagement.Server.DTOs;
using InventoryManagement.Server.Entities;

namespace InventoryManagement.Server.Services.PurchaseService
{
    public interface IPurchaseService
    {
        Task<IEnumerable<PurchaseDto>> GetAllPurchasesAsync();
        Task<PurchaseDto?> GetPurchaseByIdAsync(int id);
        Task<PurchaseDto?> CreatePurchaseAsync(CreatePurchaseDto createPurchaseDto);
        Task<IEnumerable<PurchaseDto>> GetPurchasesByProductIdAsync(int productId);
    }
}
