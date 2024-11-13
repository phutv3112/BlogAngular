using BlogAngular.Api.Models.Domain;

namespace BlogAngular.Api.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<AppUser?> FindByEmailAsync(string email);
        Task<AppUser?> FindByIdAsync(string id);
    }
}
