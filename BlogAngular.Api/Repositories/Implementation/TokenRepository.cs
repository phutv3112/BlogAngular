using BlogAngular.Api.Models;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlogAngular.Api.Repositories.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public TokenRepository(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        
        public async Task<TokenPair> CreateJwtToken(AppUser user, List<string> roles)
        {
            // Create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            // Retrieve private key from user data
            var encryptedPrivateKey = user.EncryptedPrivateKey;

            var privateKeyXml = EncryptHelper.DecryptData(encryptedPrivateKey, _configuration); 
            var rsa = RSA.Create();
            rsa.FromXmlString(privateKeyXml);

            // Create signing credentials using RSA private key
            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            // Create JWT
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: signingCredentials
            );

            var refreshToken = Guid.NewGuid().ToString();

            if (user.RefreshToken != null)
            {
                user.RefreshToken = refreshToken;
                await _userManager.UpdateAsync(user);
            }
            else
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
                await _userManager.UpdateAsync(user);
            }
            var tokenPair = new TokenPair
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            };
            return tokenPair;
        }
    }
}
