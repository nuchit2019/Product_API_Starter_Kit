using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.category.Application.DTOs
{

    public record CategoryCreateDTO(
       [Required(ErrorMessage = "Category name is required."),StringLength(100, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 100 characters.")]
        string Name,

       string? Description,

        bool? isActive

  );

}
