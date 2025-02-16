using BlogAngular.Api.Models.Domain;

namespace BlogAngular.Api.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<AppUser?> FindByEmailAsync(string email);
        Task<AppUser?> FindByIdAsync(string id);
        Task<bool> UpdateUser(AppUser user);
        Task<int> CountUsers();
        Task DeleteAsync(AppUser user);
        Task<IEnumerable<AppUser>> GetAllUserAsync(string? query = null, int? pageNumer = 1, int? pageSize = 10);
    }
}
