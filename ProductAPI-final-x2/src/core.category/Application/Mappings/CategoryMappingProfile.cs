using AutoMapper;
using core.category.Application.DTOs;
using core.category.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace core.category.Application.Mappings
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryResponseDTO>();
            CreateMap<CategoryCreateDTO, Category>();
        }
    }
}
