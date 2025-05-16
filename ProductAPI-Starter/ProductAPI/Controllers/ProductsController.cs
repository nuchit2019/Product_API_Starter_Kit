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
            // TODO 01 [Controller]: Implement ...
            return null;
        }
         

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductResponseDTO>>> Get(int id)
        {
            // TODO 02 [Controller]: Implement ...
            return null;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductResponseDTO>>> Post([FromBody] ProductCreateDTO dto)
        {

            // TODO 03 [Controller]: Implement ...
            return null;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Put(int id, [FromBody] ProductUpdateDTO dto)
        {
            // TODO 04 [Controller]: Implement ...
            return null;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            // TODO 05 [Controller]: Implement ...
            return null;
        }
    }
}
