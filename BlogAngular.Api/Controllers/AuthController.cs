using BlogAngular.Api.Models.Dtos.AuthDtos;
using BlogAngular.Api.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlogAngular.Api.Services;
using Newtonsoft.Json.Linq;
using System.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Helpers;

namespace BlogAngular.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly GoogleAuthService _googleAuthService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly FacebookAuthService _facebookAuthService;
        private readonly IEmailSender _emailSender;
        private readonly CacheService _cacheService;

        public AuthController(UserManager<AppUser> userManager, ITokenRepository tokenRepository, 
            GoogleAuthService googleAuthService, IHttpClientFactory httpClientFactory, 
            IConfiguration configuration, FacebookAuthService facebookAuthService, 
            IEmailSender emailSender, CacheService cacheService)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _googleAuthService = googleAuthService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _facebookAuthService = facebookAuthService;
            _emailSender = emailSender;
            _cacheService = cacheService;
        }
        private string GetUsernameFromEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            var atIndex = email.IndexOf('@');
            return atIndex > 0 ? email.Substring(0, atIndex) : email;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var (publicKey, privateKey) = EncryptHelper.GenerateRsaKeyPair();
            var encryptedPrivateKey = EncryptHelper.EncryptData(privateKey, _configuration);
            var user = new AppUser
            {
                Email = request.Email?.Trim(),
                UserName = GetUsernameFromEmail(request.Email?.Trim()),
                EncryptedPrivateKey = encryptedPrivateKey,
                PublicKey = publicKey
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user, "Reader");
                if (result.Succeeded)
                {
                    var token = await _tokenRepository.CreateJwtToken(user, new List<string> { "Reader" });
                    var response = new LoginResponseDto
                    {
                        Email = request.Email,
                        Roles = new List<string> { "Reader" },
                        TokenPair = token
                    };
                    return Ok(response);
                }
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors });
        }

        [HttpPost("register-mfa")]
        public async Task<IActionResult> RegisterMfa(RegisterRequestDto request)
        {
            var userFound = await _userManager.FindByEmailAsync(request.Email);
            if (userFound is null)
            {
                var (publicKey, privateKey) = EncryptHelper.GenerateRsaKeyPair();
                var encryptedPrivateKey = EncryptHelper.EncryptData(privateKey, _configuration);
                var user = new AppUser
                {
                    Email = request.Email?.Trim(),
                    UserName = GetUsernameFromEmail(request.Email?.Trim()),
                    EncryptedPrivateKey = encryptedPrivateKey,
                    PublicKey = publicKey,
                    EmailConfirmed = false,
                    LockoutEnabled = false
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(user, "Reader");
                    if (result.Succeeded)
                    {
                        var otp = StringExtensions.GenerateSecureRandomNumericCode();
                        var emailBody = StringExtensions.GetOtpEmailTemplate(otp);

                        await _emailSender.SendEmailAsync(request.Email, "Your OTP Code", emailBody);
                        _cacheService.SetCacheWithExpiration(request.Email, otp, TimeSpan.FromMinutes(5));
                        return Ok("Send mail successfully!");
                    }
                }
                return BadRequest("Register failed. Please try again!");
            }
            else
            {
                return BadRequest("Email already exist!");
            }
        }
        [HttpPost("register-mfa-verify")]
        public async Task<IActionResult> RegisterMfaVerify(OtpDto request)
        {
            var otp = _cacheService.GetCache<string>(request.Email);
            var valid = _cacheService.IsCacheKeyValid(request.Email);

            if (valid)
            {
                if (otp == request.Otp)
                {
                    var user = await _userManager.FindByEmailAsync(request.Email);
                    
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    var roles = await _userManager.GetRolesAsync(user);

                    var token = await _tokenRepository.CreateJwtToken(user, roles.ToList());
                    var response = new LoginResponseDto
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        TokenPair = token
                    };
                    _cacheService.RemoveCache(request.Email);
                    return Ok(response);
                }
                else
                {
                    return BadRequest("OTP is incorrect!");
                }
            }
            else
            {
                return BadRequest("OTP is expired!");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null)
            {
                var check = await _userManager.CheckPasswordAsync(user, request.Password);
                if (check)
                {
                    if (user.EmailConfirmed)
                    {
                        if (user.LockoutEnabled)
                        {
                            return BadRequest("Account is locked!");
                        }
                        var roles = await _userManager.GetRolesAsync(user);
                        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);
                        await _userManager.UpdateAsync(user);

                        var token = await _tokenRepository.CreateJwtToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            Email = request.Email,
                            Roles = roles.ToList(),
                            TokenPair = token
                        };
                        return Ok(response);
                    }
                    else
                    {
                        var otp = StringExtensions.GenerateSecureRandomNumericCode();
                        var emailBody = StringExtensions.GetOtpEmailTemplate(otp);

                        await _emailSender.SendEmailAsync(request.Email, "Your OTP Code", emailBody);
                        _cacheService.SetCacheWithExpiration(request.Email, otp, TimeSpan.FromMinutes(5));
                        return BadRequest("Please open your mail to verify!");
                    }

                }
                else
                {
                    return BadRequest("Password is incorrect!");
                }

            }   
            else
            {
                return BadRequest("User not found!");
            }
        }

        [HttpPost("login-mfa")]
        public async Task<IActionResult> LoginMfa(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null)
            {
                var check = await _userManager.CheckPasswordAsync(user, request.Password);
                if (check)
                {
                    var otp = StringExtensions.GenerateSecureRandomNumericCode();
                    var emailBody = StringExtensions.GetOtpEmailTemplate(otp);
                    //await _emailSender.SendEmailAsync(request.Email, "OTP", $"Your OTP is: {otp}");
                    await _emailSender.SendEmailAsync(request.Email, "Your OTP Code", emailBody);
                    _cacheService.SetCacheWithExpiration(request.Email, otp, TimeSpan.FromMinutes(5));
                    return Ok(check);
                }
                else
                {
                    return BadRequest("Password is incorrect!");
                }
            }
            else
            {
                return BadRequest("User not found!");
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromQuery(Name = "uid")] string userId, [FromQuery(Name = "rt")] string refreshToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (user.RefreshToken != refreshToken)
                {
                    return BadRequest("Refresh token not match!");
                }
                else
                {
                    if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
                    {
                        return BadRequest("Refresh token expired, please login!");
                    }
                    else
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        var tokenPair = await _tokenRepository.CreateJwtToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            Email = user.Email,
                            Roles = roles.ToList(),
                            TokenPair = tokenPair
                        };
                        return Ok(response);
                    }
                }

            }
            return BadRequest("User not exist in system!");
        }
        [HttpPost("lock-account")]
        public async Task<IActionResult> LockAccount(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.LockoutEnabled = true;
                await _userManager.UpdateAsync(user);
                return Ok("Lock account successfully!");
            }
            else
            {
               return BadRequest("User not found!");
            }
        }

        [HttpPost("unlock-account")]
        public async Task<IActionResult> UnLockAccount(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var otp = StringExtensions.GenerateSecureRandomNumericCode();
                var emailBody = StringExtensions.GetOtpEmailTemplate(otp);
                await _emailSender.SendEmailAsync(email, "Your OTP Code", emailBody);
                _cacheService.SetCacheWithExpiration(email, otp, TimeSpan.FromMinutes(5));
                return Ok("Send Otp to unclock account successfully!");
            }
            else
            {
                return BadRequest("User not found!");
            }
        }

        [HttpPost("unlock-account-verify")]
        public async Task<IActionResult> UnLockAccountVerify(OtpDto request)
        {
            var otp = _cacheService.GetCache<string>(request.Email);
            var valid = _cacheService.IsCacheKeyValid(request.Email);
            if (valid)
            {
                if (otp == request.Otp)
                {
                    var user = await _userManager.FindByEmailAsync(request.Email);
                    if (user != null)
                    {
                        user.LockoutEnabled = false;
                        await _userManager.UpdateAsync(user);
                        _cacheService.RemoveCache(request.Email);
                    }
                    return Ok("Unlock successfully, please login again!");
                }
                else
                {
                    return BadRequest("OTP is incorrect!");
                }
            }
            else
            {
                return BadRequest("OTP is expired!");
            }
        }

        [HttpPost("login-mfa-verify")]
        public async Task<IActionResult> LoginMfaVerify(OtpDto request)
        {
            var otp = _cacheService.GetCache<string>(request.Email);
            var valid = _cacheService.IsCacheKeyValid(request.Email);
            if (valid)
            {
                if (otp == request.Otp)
                {
                    var user = await _userManager.FindByEmailAsync(request.Email);
                    if(user != null && user.EmailConfirmed == false)
                    {
                        user.EmailConfirmed = true;
                        await _userManager.UpdateAsync(user);
                    }
                    var roles = await _userManager.GetRolesAsync(user);

                    var token = await _tokenRepository.CreateJwtToken(user, roles.ToList());
                    var response = new LoginResponseDto
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        TokenPair = token
                    };
                    _cacheService.RemoveCache(request.Email);
                    return Ok(response);
                }
                else
                {
                    return BadRequest("OTP is incorrect!");
                }
            }
            else
            {
                return BadRequest("OTP is expired!");
            }
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            var payload = await _googleAuthService.VerifyGoogleTokenAsync(request.IdToken);

            if (payload != null)
            {
                // Token is valid, handle login logic here
                var userFound = await _userManager.FindByEmailAsync(payload.Email);
                if(userFound == null)
                {
                    var user = new AppUser
                    {
                        Email = payload.Email,
                        UserName = payload.Email
                    };
                    var result = await _userManager.CreateAsync(user, payload.Picture);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddToRoleAsync(user, "Reader");
                        if (result.Succeeded)
                        {
                            userFound = user;
                        }
                        else
                        {
                            if (result.Errors.Any())
                            {
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError("", error.Description);
                                }
                            }
                        }
                    }
                }
                var roles = await _userManager.GetRolesAsync(userFound);

                var token = await _tokenRepository.CreateJwtToken(userFound, roles.ToList());
                var response = new LoginResponseDto
                {
                    Email = payload.Email,
                    Roles = roles.ToList(),
                    TokenPair = token
                };

                return Ok(response);
            }

            return Unauthorized();
        }

        private async Task<bool> ValidateFacebookToken(string authToken)
        {
            var client = _httpClientFactory.CreateClient();
            var appId = _configuration["Authentication:Facebook:AppId"];
            var appSecret = _configuration["Authentication:Facebook:AppSecret"];

            var response = await client.GetStringAsync(
                $"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={appId}|{appSecret}");

            var jsonResponse = JObject.Parse(response);
            var isValid = jsonResponse["data"]?["is_valid"]?.Value<bool>() ?? false;

            return isValid;
        }

        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginRequest request)
        {
            var userInfo = await _facebookAuthService.VerifyFacebookTokenAsync(request.AuthToken);

            if (userInfo != null)
            {
                var userFound = await _userManager.FindByEmailAsync(userInfo.Email);
                if (userFound == null)
                {
                    var user = new AppUser
                    {
                        Email = userInfo.Email,
                        UserName = userInfo.Email
                    };
                    var result = await _userManager.CreateAsync(user, userInfo.Id);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddToRoleAsync(user, "Reader");
                        if (result.Succeeded)
                        {
                            userFound = user;
                        }
                        else
                        {
                            if (result.Errors.Any())
                            {
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError("", error.Description);
                                }
                            }
                        }
                    }
                }
                var roles = await _userManager.GetRolesAsync(userFound);

                var token = await _tokenRepository.CreateJwtToken(userFound, roles.ToList());
                var response = new LoginResponseDto
                {
                    Email = userInfo.Email,
                    Roles = roles.ToList(),
                    TokenPair = token
                };

                return Ok(response);
            }

            return Unauthorized();

        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Email invalid!" });
            }
            var otp = StringExtensions.GenerateSecureRandomNumericCode();
            var emailBody = StringExtensions.GetOtpEmailTemplate(otp);
            await _emailSender.SendEmailAsync(user.Email, "Your OTP Code", emailBody);
            _cacheService.SetCacheWithExpiration(user.Email, otp, TimeSpan.FromMinutes(5));

            return Ok(new { message = "Send Otp successfully!" });
        }
        [HttpPost("verify-forgot-password")]
        public IActionResult VerifyForgotPassword(OtpDto request)
        {
            try
            {
                var otp = _cacheService.GetCache<string>(request.Email);
                var valid = _cacheService.IsCacheKeyValid(request.Email);
                if (valid)
                {
                    if (otp == request.Otp)
                    {
                        _cacheService.RemoveCache(request.Email);
                        return Ok("Verify successfully!");
                    }
                    else
                    {
                        return BadRequest("OTP is incorrect!");
                    }
                }
                else
                {
                    return BadRequest("OTP is expired!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Send mail to verify failure!");
            }
        }
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto, [FromQuery(Name = "email")] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("User not exist");
            }
            user.PasswordHash = new PasswordHasher<AppUser>().HashPassword(null, resetPasswordDto.Password);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { message = "Reset password successfully!" });
            }
            else
            {
                return BadRequest("Reset password fail");
            }
        }
    }
}
