using System.ComponentModel.DataAnnotations;

namespace core.product.Application.DTOs
{
    public record ProductCreateDTO(
        [Required(ErrorMessage = "Product name is required."),StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters.")]
        string Name,

        string? Description,

        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0.")]  
        decimal Price,

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number.")]
        int Stock

   ); 

}
