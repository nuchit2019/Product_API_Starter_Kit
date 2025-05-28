using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.category.Application.DTOs
{
    public record CategoryResponseDTO(
       int CategoryId,
       string Name,
       string? Description,
       bool? IsActive 

  );

}
