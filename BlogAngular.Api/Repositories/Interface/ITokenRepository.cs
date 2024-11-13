using BlogAngular.Api.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace BlogAngular.Api.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(AppUser user, List<string> roles);
    }
}
