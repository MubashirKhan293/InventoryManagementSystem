using InventoryManagement.Server.DTOs;

namespace InventoryManagement.Server.Services.SaleService
{
    public interface ISaleService
    {
        Task<IEnumerable<SaleDto>> GetAllSalesAsync();
        Task<SaleDto?> GetSaleByIdAsync(int id);
        Task<SaleDto?> CreateSaleAsync(CreateSaleDto createSaleDto);
        Task<IEnumerable<SaleDto>> GetSalesByProductIdAsync(int productId);
    }
}
