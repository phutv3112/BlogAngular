using BlogAngular.Api.Data;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Models.Dtos.CategoryDtos;
using BlogAngular.Api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BlogAngular.Api.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountCategories()
        {
            return await _context.Categories.CountAsync();
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task Delete(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category?> FindByIdAsync(Guid id)
        {
            return await _context.Categories.FindAsync(id);

        }
        public async Task<List<CategoryCountPosts>> GetCategoriesAndCountPosts() {
            var categories = _context.Categories.Include(c => c.BlogPosts).ToList();
            var result = new List<CategoryCountPosts>();
            foreach (var category in categories)
            {
                var countPosts = category.BlogPosts.Count;
                result.Add(new CategoryCountPosts { CateId = category.Id ,Name = category.Name, CountPosts = countPosts });
            }
            result = result.OrderByDescending(c => c.CountPosts).ToList();
            result = result.Take(8).ToList();
            return result;
        }
        public async Task<IEnumerable<Category>> GetAllAsync(string? query = null, string? sortBy = null, string? sortDirection = null,
            int? pageNumer = 1, int? pageSize = 10)
        {
            // Query
            var categories = _context.Categories.AsQueryable();

            // Fliter
            if(!string.IsNullOrEmpty(query))
            {
                categories = categories.Where(x => x.Name.Contains(query));
            }
            
            // Sorting
            if(!string.IsNullOrEmpty(sortBy))
            {
                if(string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    categories = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) ? categories.OrderBy(c => c.Name) : categories.OrderByDescending(c => c.Name);
                }
                else if(string.Equals(sortBy, "Url", StringComparison.OrdinalIgnoreCase))
                {
                    categories = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) ? categories.OrderBy(c => c.UrlHandle) : categories.OrderByDescending(c => c.UrlHandle);
                }
            }
            else
            {
                categories = categories.OrderByDescending(c => c.UpdatedDate);
            }

            // Pagination
            if(pageNumer <= 1)
            {
                pageNumer = 1;
            }
            var skip = (pageNumer - 1) * pageSize;
            categories = categories.Skip(skip ?? 0).Take(pageSize ?? 10);

            return await categories.ToListAsync(); ;
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existCategory = await _context.Categories.FindAsync(category.Id);
            if (existCategory != null)
            {
                category.UpdatedDate = DateTime.Now;
                _context.Entry(existCategory).CurrentValues.SetValues(category);
                await _context.SaveChangesAsync();
                return existCategory;
            }
            return null;
        }

    }
}
