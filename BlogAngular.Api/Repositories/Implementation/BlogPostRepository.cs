using BlogAngular.Api.Data;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Models.Dtos.CategoryDtos;
using BlogAngular.Api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BlogAngular.Api.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly AppDbContext _context;

        public BlogPostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountPosts()
        {
            return await _context.BlogPosts.CountAsync();
        }
        public async Task<int> CountCategoryPosts(Guid cateId)
        {
            var category = await _context.Categories.Include(c => c.BlogPosts).FirstOrDefaultAsync(c => c.Id == cateId);
            if(category == null)
            {
                return 0;
            }
            return category.BlogPosts.Count();

        }
        public async Task<IEnumerable<BlogPost>> GetAllAsync(int? pageNumber = 1, int? pageSize = 3)
        {
            var blogPosts = _context.BlogPosts.AsQueryable();

            blogPosts = blogPosts.Include(b => b.Categories).OrderByDescending(d => d.UpdatedDate);
            // Pagination
            if (pageNumber <= 1)
            {
                pageNumber = 1;
            }
            var skip = (pageNumber - 1) * pageSize;
            blogPosts = blogPosts.Skip(skip ?? 0).Take(pageSize ?? 3);
            return await blogPosts.ToListAsync();
        }

        public async Task<IEnumerable<BlogPost>> GetByCategoryAsync(Guid categoryId, int? pageNumber = 1, int? pageSize = 3)
        {
            var blogPosts = _context.BlogPosts.AsQueryable();

            blogPosts = blogPosts.Include(b => b.Categories).OrderByDescending(d => d.UpdatedDate);
            blogPosts = blogPosts.Where(b => b.Categories.Any(c => c.Id == categoryId));
            // Pagination
            if (pageNumber <= 1)
            {
                pageNumber = 1;
            }
            var skip = (pageNumber - 1) * pageSize;
            blogPosts = blogPosts.Skip(skip ?? 0).Take(pageSize ?? 3);
            return await blogPosts.ToListAsync();
        }

        public async Task<BlogPost?> FindByIdAsync(Guid id)
        {
            return await _context.BlogPosts.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _context.BlogPosts.AddAsync(blogPost);
            await _context.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existBlog = await _context.BlogPosts.FirstOrDefaultAsync(b => b.Id == blogPost.Id);
            if (existBlog != null)
            {
                blogPost.UpdatedDate = DateTime.Now;
                _context.Entry(existBlog).CurrentValues.SetValues(blogPost);
                await _context.SaveChangesAsync();
                return blogPost;
            }
            return null;
        }

        public async Task DeleteAsync(BlogPost blogPost)
        {
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
        }

        public async Task<BlogPost?> GetByUrlAsync(string url)
        {
            return await _context.BlogPosts.FirstOrDefaultAsync(b => b.UrlHandle == url);
        }
    }
}
