using InventoryManagement.Server.DTOs;
using InventoryManagement.Server.Services.ProductService;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("GetProductById")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductInputDto createProductDto)
        {
            var createdProduct = await _productService.CreateProductAsync(createProductDto);
            return Ok(createdProduct);
        }

        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, ProductInputDto updateProductDto)
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, updateProductDto);
            if (updatedProduct == null)
                return NotFound();

            return Ok(updatedProduct);
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound();

            return Ok(new { success = true, message = "Product deleted successfully" });
        }

        [HttpGet("ProductNameExistsAsync")]
        public async Task<bool> ProductNameExistsAsync(string name)
        {
            var exists = await _productService.ProductNameExistsAsync(name);
            return exists;
        }
    }
}
