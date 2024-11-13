using AutoMapper;
using AutoMapper.Configuration.Annotations;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Models.Dtos.CategoryDtos;
using BlogAngular.Api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAngular.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? query, [FromQuery]string? sortBy, 
            [FromQuery]string? sortDirection, [FromQuery]int? pageNumber, [FromQuery] int? pageSize)
        {
            var result = await _categoryRepository.GetAllAsync(query, sortBy, sortDirection, pageNumber, pageSize);
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(result);
            return Ok(categoryDtos);
        }
        [HttpGet("categories-count-posts")]
        public async Task<IActionResult> GetCategoriesAndCountPosts()
        {
            var result = await _categoryRepository.GetCategoriesAndCountPosts();
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Writer")]
        public async Task<IActionResult> CreateAsync(CreateCategoryRequestDto requestDto)
        {
            var category = new Category
            {
                Name = requestDto.Name,
                UrlHandle = requestDto.UrlHandle
            };
            var result = await _categoryRepository.CreateAsync(category);
            var categoryDto = _mapper.Map<CategoryDto>(result);
            return Ok(categoryDto);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.FindByIdAsync(id);
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountCategories()
        {
            var count = await _categoryRepository.CountCategories();
            return Ok(count);
        }
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin, Writer")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateCategoryRequestDto requestDto)
        {
            var category = new Category
            {
                Id = id,
                Name = requestDto.Name,
                UrlHandle = requestDto.UrlHandle
            };
            category = await _categoryRepository.UpdateAsync(category);
            if(category == null)
            {
                return NotFound();
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Writer")]
        public async Task<IActionResult>DeleteCategory(Guid id)
        {
            var category = await _categoryRepository.FindByIdAsync(id);
            if(category == null)
            {
                return NotFound();
            }
            await _categoryRepository.Delete(category);
            return Ok();
        }

    }
}
