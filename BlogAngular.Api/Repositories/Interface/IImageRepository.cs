using BlogAngular.Api.Models.Domain;

namespace BlogAngular.Api.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
        Task<IEnumerable<BlogImage>> GetAll();
        Task<BlogImage?> GetById(Guid id);
        Task Delete(BlogImage blogImage);
    }
}
