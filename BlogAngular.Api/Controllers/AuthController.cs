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

        public AuthController(UserManager<AppUser> userManager, ITokenRepository tokenRepository, 
            GoogleAuthService googleAuthService, IHttpClientFactory httpClientFactory, 
            IConfiguration configuration, FacebookAuthService facebookAuthService, IEmailSender emailSender)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _googleAuthService = googleAuthService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _facebookAuthService = facebookAuthService;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var user = new AppUser
            {
                Email = request.Email?.Trim(),
                UserName = request.Email?.Trim()
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user, "Reader");
                if (result.Succeeded)
                {
                    var token = _tokenRepository.CreateJwtToken(user, new List<string> { "Reader" });
                    var response = new LoginResponseDto
                    {
                        Email = request.Email,
                        Roles = new List<string> { "Reader" },
                        Token = token
                    };
                    return Ok(response);
                }
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors });
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
                    var roles = await _userManager.GetRolesAsync(user);

                    var token = _tokenRepository.CreateJwtToken(user, roles.ToList());
                    var response = new LoginResponseDto
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = token
                    };
                    return Ok(response);
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
            //ModelState.AddModelError("", "Email or password incorrect!");
            //return ValidationProblem(ModelState);
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

                var token = _tokenRepository.CreateJwtToken(userFound, roles.ToList());
                var response = new LoginResponseDto
                {
                    Email = payload.Email,
                    Roles = roles.ToList(),
                    Token = token
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

                var token = _tokenRepository.CreateJwtToken(userFound, roles.ToList());
                var response = new LoginResponseDto
                {
                    Email = userInfo.Email,
                    Roles = roles.ToList(),
                    Token = token
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
            var clientAddress = _configuration["BaseAddress:ClientAddress"];
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var frontendResetPasswordUrl = $"{clientAddress}/account/reset-password?code={code}&email={forgotPasswordDto.Email}";

            await _emailSender.SendEmailAsync(
                forgotPasswordDto.Email,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(frontendResetPasswordUrl)}'>clicking here</a>."
            );

            return Ok(new { message = "Open your email and click to reset password!" });
        }
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto, [FromQuery(Name = "email")] string email, [FromQuery(Name = "code")] string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || code == null)
            {
                return BadRequest();
            }
            var decode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ResetPasswordAsync(user, decode, resetPasswordDto.Password);
            if (result.Succeeded)
            {
                return Ok(new { message = "Reset password successfully!" });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
