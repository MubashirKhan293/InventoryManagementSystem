using InventoryManagement.Server.DTOs;
using InventoryManagement.Server.Services.PurchaseService;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchasesController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet("GetAllPurchases")]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> GetAllPurchases()
        {
            var purchases = await _purchaseService.GetAllPurchasesAsync();
            return Ok(purchases);
        }

        [HttpGet("GetPurchaseById")]
        public async Task<ActionResult<PurchaseDto>> GetPurchaseById(int id)
        {
            var purchase = await _purchaseService.GetPurchaseByIdAsync(id);
            if (purchase == null)
                return NotFound();

            return Ok(purchase);
        }

        [HttpPost("CreatePurchase")]
        public async Task<ActionResult<PurchaseDto>> CreatePurchase(CreatePurchaseDto createPurchaseDto)
        {
            try
            {
                var createdPurchase = await _purchaseService.CreatePurchaseAsync(createPurchaseDto);
                return Ok(createdPurchase);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetPurchasesByProduct")]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> GetPurchasesByProduct(int productId)
        {
            var purchases = await _purchaseService.GetPurchasesByProductIdAsync(productId);
            return Ok(purchases);
        }
    }
    //[HttpPut("UpdatePurchaseStatus")]
    //public async Task<ActionResult<PurchaseDto>> UpdatePurchaseStatus(int id, UpdatePurchaseStatusDto statusDto)
    //{
    //    try
    //    {
    //        var updatedPurchase = await _purchaseService.UpdatePurchaseStatusAsync(id, statusDto);
    //        if (updatedPurchase == null)
    //            return NotFound();

    //        return Ok(updatedPurchase);
    //    }
    //    catch (System.Exception ex)
    //    {
    //        return BadRequest(new { message = ex.Message });
    //    }
    //}
}
