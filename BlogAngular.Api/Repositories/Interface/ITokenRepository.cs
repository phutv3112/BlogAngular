using BlogAngular.Api.Models;
using BlogAngular.Api.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace BlogAngular.Api.Repositories.Interface
{
    public interface ITokenRepository
    {
        Task<TokenPair> CreateJwtToken(AppUser user, List<string> roles);
    }
}
