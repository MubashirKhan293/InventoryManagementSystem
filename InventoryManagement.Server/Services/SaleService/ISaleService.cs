using InventoryManagement.Server.DTOs;

namespace InventoryManagement.Server.Services.SaleService
{
    public interface ISaleService
    {
        Task<List<SaleDto>> GetAllSalesAsync();
        Task<SaleDto> GetSaleByIdAsync(int id);
        Task<SaleDto> CreateSaleAsync(CreateSaleDto saleDto);
    }
}
