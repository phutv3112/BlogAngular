using BlogAngular.Api.Models.Domain;

namespace BlogAngular.Api.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync(int? pageNumer = 1, int? pageSize = 3);
        Task<BlogPost?> FindByIdAsync(Guid id);
        Task<int> CountPosts();
        Task<int> CountCategoryPosts(Guid cateId);
        Task<BlogPost?> GetByUrlAsync(string url);
        Task<IEnumerable<BlogPost>> GetByCategoryAsync(Guid categoryId, int? pageNumber = 1, int? pageSize = 3);
        Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
        Task DeleteAsync(BlogPost blogPost);
    }
}
