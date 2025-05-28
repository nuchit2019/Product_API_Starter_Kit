using core.product.Application.DTOs;
using core.product.Application.Interfaces;
using core.api.Common;
using Microsoft.AspNetCore.Mvc; 

namespace core.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger ,IProductService productService)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet] 
      public async Task<ActionResult<ApiResponse<IEnumerable<ProductResponseDTO>>>> GetAllProducts()
        {
            var products = await _productService.GetAllAsync();
            if (products == null || !products.Any())
            {
                return NotFound(ApiResponse<IEnumerable<ProductResponseDTO>>.FailResponse("No products found.", 404));
            }
            return Ok(ApiResponse<IEnumerable<ProductResponseDTO>>.SuccessResponse(products, "Products retrieved successfully."));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductResponseDTO>>> Get(int id)
        {

            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(ApiResponse<ProductResponseDTO>.FailResponse($"Product with id {id} not found.", 404));
            }
            return Ok(ApiResponse<ProductResponseDTO>.SuccessResponse(product, "Product retrieved successfully."));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductResponseDTO>>> Post([FromBody] ProductCreateDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(ApiResponse<ProductResponseDTO>.FailResponse("Product data is required.", 400));
            }

            var createdProduct = await _productService.CreateAsync(dto);
            if (createdProduct == null)
            {
                return StatusCode(500, ApiResponse<ProductResponseDTO>.FailResponse("Failed to create product.", 500));
            }

            return CreatedAtAction(nameof(Get), new { id = createdProduct.Id },
                ApiResponse<ProductResponseDTO>.SuccessResponse(createdProduct, "Product created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Put(int id, [FromBody] ProductUpdateDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(ApiResponse<object>.FailResponse("Product data is required.", 400));
            }


            var updated = await _productService.UpdateAsync(dto);
            if (!updated)
            {
                return NotFound(ApiResponse<object>.FailResponse($"Product with id {id} not found.", 404));
            }

            return Ok(ApiResponse<object>.SuccessResponse(null, "Product updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var deleted = await _productService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(ApiResponse<object>.FailResponse($"Product with id {id} not found.", 404));
            }

            return Ok(ApiResponse<object>.SuccessResponse(null, "Product deleted successfully."));
        }
    }
}
