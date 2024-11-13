using BlogAngular.Api.Data;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Repositories.Interface;
using Microsoft.AspNetCore.Identity;

namespace BlogAngular.Api.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<AppUser?> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<AppUser?> FindByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }
    }
}
