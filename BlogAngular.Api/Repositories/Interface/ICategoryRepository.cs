using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Models.Dtos.CategoryDtos;

namespace BlogAngular.Api.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync(string? query = null, string? sortBy = null, string? sortDirection = null,
            int? pageNumer = 1, int? pageSize = 10);
        Task<Category?> FindByIdAsync(Guid id);
        Task<int> CountCategories();
        Task<Category?> UpdateAsync(Category category);
        Task Delete(Category category);
        Task<List<CategoryCountPosts>> GetCategoriesAndCountPosts();
    }
}
