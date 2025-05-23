using InventoryManagement.Server.DTOs;
using InventoryManagement.Server.Services.SaleService;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet("GetAllSales")]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetAllSales()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpGet("GetSaleById")]
        public async Task<ActionResult<SaleDto>> GetSaleById(int id)
        {
            var sale = await _saleService.GetSaleByIdAsync(id);
            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        [HttpPost("CreateSale")]
        [Consumes("application/json")]
        public async Task<ActionResult<SaleDto>> CreateSale([FromBody] CreateSaleDto saleDto)
        {
            try
            {
                var createdSale = await _saleService.CreateSaleAsync(saleDto);
                return Ok(createdSale);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
