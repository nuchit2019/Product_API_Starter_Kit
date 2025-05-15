using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Interfaces;
using ProductAPI.Common;

namespace ProductAPI.Controllers
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
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetAllProducts()
        { 
            try
            {
                var products = await _productService.GetAllAsync();

                var response = ApiResponse<IEnumerable<ProductResponseDTO>>.SuccessResponse(
                    data: products,
                    message: "Products retrieved successfully",
                    statusCode: StatusCodes.Status200OK
                );

                //throw new Exception("Test Exception GetAllProducts---------------");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetAllProducts");
                var errorResponse = ApiResponse<object>.FailResponse(
                    message: "An unexpected error occurred while fetching products.",
                    statusCode: StatusCodes.Status500InternalServerError,
                    error: ex.Message // หรือจะรวมกับ StackTrace ก็ได้
                );

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }

        }

        [HttpGet("api/Products2")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductResponseDTO>>>> Get()
        {
            var products = await _productService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<ProductResponseDTO>>.SuccessResponse(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductResponseDTO>>> Get(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return product != null
                ? Ok(ApiResponse<ProductResponseDTO>.SuccessResponse(product))
                : NotFound(ApiResponse<ProductResponseDTO>.FailResponse("Product not found", 404));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductResponseDTO>>> Post([FromBody] ProductCreateDTO dto)
        {
            var product = await _productService.CreateAsync(dto);

            //throw new Exception("Test Exception CreatedAtAction -----------------------");
            return CreatedAtAction(nameof(Get), new { id = product.Id },  ApiResponse<ProductResponseDTO>.SuccessResponse(product, "Product created", 201));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Put(int id, [FromBody] ProductUpdateDTO dto)
        {
            if (id != dto.Id)
                return BadRequest(ApiResponse<object>.FailResponse("Invalid product ID", 400));

            var result = await _productService.UpdateAsync(dto);
            return result
                ? Ok(ApiResponse<object>.SuccessResponse(null, "Product updated"))
                : NotFound(ApiResponse<object>.FailResponse("Product not found", 404));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            return result
                ? Ok(ApiResponse<object>.SuccessResponse(null, "Product deleted"))
                : NotFound(ApiResponse<object>.FailResponse("Product not found", 404));
        }
    }
}
