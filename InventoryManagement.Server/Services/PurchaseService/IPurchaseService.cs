using InventoryManagement.Server.Context;
using InventoryManagement.Server.DTOs;
using InventoryManagement.Server.Entities;

namespace InventoryManagement.Server.Services.PurchaseService
{
    public interface IPurchaseService
    {
        Task<List<PurchaseDto>> GetAllPurchasesAsync();
        Task<PurchaseDto> GetPurchaseByIdAsync(int id);
        Task<PurchaseDto> CreatePurchaseAsync(CreatePurchaseDto purchaseDto);
        Task<PurchaseDto> UpdatePurchaseStatusAsync(int id, UpdatePurchaseStatusDto statusDto);
    }
}
