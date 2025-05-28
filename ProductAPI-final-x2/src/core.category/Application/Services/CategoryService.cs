using AutoMapper;
using core.category.Application.DTOs;
using core.category.Application.Interfaces;
using core.category.Domain.Entities;
using core.category.Domain.Interfaces;

namespace core.category.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<int> AddCategoryAsync(CategoryCreateDTO category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            var entity = _mapper.Map<Category>(category);

            return await _categoryRepository.AddAsync(entity);
        }

        public async Task<CategoryResponseDTO?> GetCategoryByIdAsync(int id)
        {
            var cate = await _categoryRepository.GetByIdAsync(id);
            if (cate == null)
                return null;  

            return _mapper.Map<CategoryResponseDTO>(cate);
        }

        public async Task<IEnumerable<CategoryResponseDTO>> GetAllCategoriesAsync()
        {
            var cates = await _categoryRepository.GetAllAsync();

            //return cates.Select(MapToDTO);
            return _mapper.Map<IEnumerable<CategoryResponseDTO>>(cates);
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            category.UpdatedAt = DateTime.UtcNow;
            return await _categoryRepository.UpdateAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }


        //public static CategoryResponseDTO MapToDTO(Category category)
        //{
        //    if (category == null) throw new ArgumentNullException(nameof(category));

        //    return new CategoryResponseDTO
        //    (
        //        category.CategoryId,
        //        category.Name,
        //        category.Description,
        //        category.IsActive
        //    );
        //}
    }
     
}
