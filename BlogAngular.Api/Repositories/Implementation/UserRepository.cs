using BlogAngular.Api.Data;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogAngular.Api.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(UserManager<AppUser> userManager, AppDbContext context,
            ILogger<UserRepository> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
        public async Task<AppUser?> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        public async Task<int> CountUsers()
        {
            return await _context.Users.CountAsync();
        }
        public async Task DeleteAsync(AppUser user)
        {
            try
            {
                _context.PostLikes.RemoveRange(user.PostLikes);
                _context.Comments.RemoveRange(user.Comments);
                _context.BlogPosts.RemoveRange(user.BlogPosts);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("UserRepository DeleteAsync " + ex.Message);
            }

        }
        public async Task<IEnumerable<AppUser>> GetAllUserAsync(string? query = null, int? pageNumer = 1, int? pageSize = 10)
        {
            var users = _context.Users.AsQueryable();

            // Fliter
            if (!string.IsNullOrEmpty(query))
            {
                users = users.Where(x => x.FullName.Contains(query)
                || x.UserName.Contains(query) || x.Email.Contains(query));
            }

            users = users
                .OrderByDescending(d => d.UpdatedDate);
            // Pagination
            if (pageNumer <= 1)
            {
                pageNumer = 1;
            }
            var skip = (pageNumer - 1) * pageSize;
            users = users.Skip(skip ?? 0).Take(pageSize ?? 5);
            return await users.ToListAsync();
        }
        public async Task<AppUser?> FindByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }
        public async Task<bool> UpdateUser(AppUser user)
        {
            try
            {
                var existUser = await _context.Users.FirstOrDefaultAsync(b => b.Id == user.Id);
                if (existUser != null)
                {
                    user.UpdatedDate = DateTime.Now;
                    _context.Entry(existUser).CurrentValues.SetValues(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("UserRepository UpdateAsync " + ex.Message);
                return false;
            }

        }
    }
}
