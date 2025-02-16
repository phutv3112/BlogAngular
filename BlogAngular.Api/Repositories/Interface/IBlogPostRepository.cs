using BlogAngular.Api.Models.Domain;

namespace BlogAngular.Api.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync(string? query = null, int ? pageNumer = 1, int? pageSize = 3);
        Task<IEnumerable<BlogPost>> GetAllPostsUserLiked(string userId, int? pageNumer = 1, int? pageSize = 3);
        Task<IEnumerable<BlogPost>> GetAllPostWithLikesAsync(int? pageSize = 5);
        Task<BlogPost?> FindByIdAsync(Guid id);
        Task<int> CountPosts();
        Task<int> CountCategoryPosts(Guid cateId);
        Task<int> CountPostsOfAuthor(string authorId);
        Task<BlogPost?> GetByUrlAsync(string url);
        Task<IEnumerable<BlogPost>> GetByCategoryAsync(Guid categoryId, int? pageNumber = 1, int? pageSize = 3);
        Task<IEnumerable<BlogPost>> GetByAuthorAsync(string AuthorId, int? pageNumber = 1, int? pageSize = 3);
        Task<IEnumerable<BlogPost>> GetPopularPosts(int? postNumbers = 3);
        Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
        Task DeleteAsync(BlogPost blogPost);
    }
}
