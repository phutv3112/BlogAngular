using BlogAngular.Api.Data;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BlogAngular.Api.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task Delete(BlogImage blogImage)
        {
            _context.BlogImages.Remove(blogImage);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
            return await _context.BlogImages.OrderByDescending(i => i.UpdatedDate).ToListAsync();
        }

        public async Task<BlogImage?> GetById(Guid id)
        {
            return await _context.BlogImages.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            if (file == null || blogImage == null || string.IsNullOrEmpty(blogImage.FileName) || string.IsNullOrEmpty(blogImage.FileExtension))
            {
                throw new ArgumentException("Invalid input parameters.");
            }

            try
            {
                // Upload image to server Images folder
                var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");

                // Check if file already exists to prevent overwriting
                if (System.IO.File.Exists(localPath))
                {
                    throw new IOException("A file with the same name already exists.");
                }

                using var stream = new FileStream(localPath, FileMode.Create);
                await file.CopyToAsync(stream);

                // Update database
                var httpRequest = _httpContextAccessor.HttpContext.Request;
                var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";
                blogImage.Url = urlPath;

                await _context.BlogImages.AddAsync(blogImage);
                await _context.SaveChangesAsync();

                return blogImage;
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here) and rethrow or handle as appropriate
                throw new Exception("An error occurred while uploading the image.", ex);
            }
        }

    }
}
