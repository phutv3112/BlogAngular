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
        private readonly ILogger<BlogPostRepository> logger;

        public BlogPostRepository(AppDbContext context, ILogger<BlogPostRepository> logger)
        {
            _context = context;
            this.logger = logger;
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
        public async Task<int> CountPostsOfAuthor(string authorId)
        {
            var posts = await _context.BlogPosts.Where(p => p.AuthorId == authorId).ToListAsync();
            if (posts == null)
            {
                return 0;
            }
            return posts.Count();

        }
        public async Task<IEnumerable<BlogPost>> GetAllAsync(string? query = null, int ? pageNumber = 1, int? pageSize = 3)
        {
            var blogPosts = _context.BlogPosts.Include(p => p.Author).AsQueryable();

            // Fliter
            if (!string.IsNullOrEmpty(query))
            {
                blogPosts = blogPosts.Where(x => x.Content.Contains(query) || x.Author.UserName.Contains(query)
                || x.Title.Contains(query) || x.ShortDescription.Contains(query));
            }

            blogPosts = blogPosts.Include(b => b.Categories)
                .OrderByDescending(d => d.UpdatedDate);
            // Pagination
            if (pageNumber <= 1)
            {
                pageNumber = 1;
            }
            var skip = (pageNumber - 1) * pageSize;
            blogPosts = blogPosts.Skip(skip ?? 0).Take(pageSize ?? 3);
            return await blogPosts.ToListAsync();
        }
        public async Task<IEnumerable<BlogPost>> GetAllPostsUserLiked(string userId, int? pageNumber = 1, int? pageSize = 3)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 3;

            // Load user and their liked posts
            var user = await _context.Users
                .Include(u => u.PostLikes)
                .ThenInclude(pl => pl.Post)
                .ThenInclude(p => p.Categories)
                .Include(u => u.PostLikes)
                .ThenInclude(pl => pl.Post.Author)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return Enumerable.Empty<BlogPost>();
            }

            // Retrieve blog posts liked by the user
            var blogPosts = user.PostLikes
                .Where(pl => pl.Post != null && !pl.IsUnLiked)
                .Select(pl => pl.Post)
                .OrderByDescending(p => p.UpdatedDate);

            // Paginate the result
            var skip = (pageNumber - 1) * pageSize;
            var paginatedPosts = blogPosts.Skip(skip ?? 0).Take(pageSize ?? 3);

            return paginatedPosts.ToList();
        }

        public async Task<IEnumerable<BlogPost>> GetAllPostWithLikesAsync(int? pageSize = 5)
        {
            var blogPosts = _context.BlogPosts
                .Include(p => p.PostLikes)
                .Include(p => p.Author)
                .Include(b => b.Categories)
                .AsQueryable();

            blogPosts = blogPosts
                .OrderByDescending(p => p.PostLikes.Count)
                .ThenByDescending(d => d.UpdatedDate); 

            blogPosts = blogPosts.Take(pageSize ?? 5);

            return await blogPosts.ToListAsync();
        }


        public async Task<IEnumerable<BlogPost>> GetByCategoryAsync(Guid categoryId, int? pageNumber = 1, int? pageSize = 3)
        {
            var blogPosts = _context.BlogPosts.AsQueryable();

            blogPosts = blogPosts.Include(b => b.Categories).Include(p => p.Author).OrderByDescending(d => d.UpdatedDate);
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
        public async Task<IEnumerable<BlogPost>> GetByAuthorAsync(string authorId, int? pageNumber = 1, int? pageSize = 3)
        {
            var blogPosts = _context.BlogPosts.AsQueryable();

            blogPosts = blogPosts.Include(b => b.Categories).Include(p => p.Author).OrderByDescending(d => d.UpdatedDate);
            blogPosts = blogPosts.Where(b => b.AuthorId == authorId);
            // Pagination
            if (pageNumber <= 1)
            {
                pageNumber = 1;
            }
            var skip = (pageNumber - 1) * pageSize;
            blogPosts = blogPosts.Skip(skip ?? 0).Take(pageSize ?? 3);
            return await blogPosts.ToListAsync();
        }
        public async Task<IEnumerable<BlogPost>> GetPopularPosts(int? postNumbers = 3)
        {
            var blogPosts = _context.BlogPosts.AsQueryable();

            blogPosts = blogPosts.Include(b => b.Categories)
                .Include(p => p.Author)
                .Include(p => p.PostLikes)
                .OrderByDescending(d => d.PostLikes.Count);
            
            blogPosts = blogPosts.Take(postNumbers ?? 3);
            return await blogPosts.ToListAsync();
        }

        public async Task<BlogPost?> FindByIdAsync(Guid id)
        {
            return await _context.BlogPosts
                .Include(p => p.Categories)
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            try
            {
                await _context.BlogPosts.AddAsync(blogPost);
                await _context.SaveChangesAsync();
                return blogPost;
            }
            catch (Exception ex)
            {
                logger.LogError("BlogPostRepository CreateAsync " + ex.Message);
                return null;
            }
            
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            try
            {
                var existBlog = await _context.BlogPosts
                    .Include(b => b.Categories)
                    .FirstOrDefaultAsync(b => b.Id == blogPost.Id);
                if (existBlog != null)
                {
                    
                    _context.Entry(existBlog).CurrentValues.SetValues(blogPost);
                    existBlog.Categories = blogPost.Categories;
                    await _context.SaveChangesAsync();
                    return blogPost;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError("BlogPostRepository UpdateAsync " + ex.Message);
                return null;
            }
            
        }

        public async Task DeleteAsync(BlogPost blogPost)
        {
            try
            {
                _context.PostLikes.RemoveRange(blogPost.PostLikes);
                _context.Comments.RemoveRange(blogPost.Comments);
                _context.BlogPosts.Remove(blogPost);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("BlogPostRepository DeleteAsync " + ex.Message);
            }

        }

        public async Task<BlogPost?> GetByUrlAsync(string url)
        {
            return await _context.BlogPosts
                .Include(b => b.Categories)
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.UrlHandle == url);
        }
    }
}
