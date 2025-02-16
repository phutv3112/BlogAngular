using AutoMapper;
using BlogAngular.Api.Helpers;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Models.Dtos.AuthDtos;
using BlogAngular.Api.Models.Dtos.BlogDtos;
using BlogAngular.Api.Models.Dtos.UserDtos;
using BlogAngular.Api.Repositories.Implementation;
using BlogAngular.Api.Repositories.Interface;
using BlogAngular.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogAngular.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly CacheService _cacheService;
        private readonly UserManager<AppUser> _userManager;

        public UsersController(IUserRepository userRepository, IMapper mapper,
            IEmailSender emailSender, CacheService cacheService, UserManager<AppUser> userManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _emailSender = emailSender;
            _cacheService = cacheService;
            _userManager = userManager;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return BadRequest("User id is required");
            }
            var user = await _userRepository.FindByIdAsync(id);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }
        [HttpGet]
        [Route("get-by-email/{email}")]
        public async Task<IActionResult> GetUserByEmail([FromRoute] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("User id is required");
            }
            var user = await _userRepository.FindByEmailAsync(email);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }
        [HttpGet]
        [Authorize]
        [Route("count")]
        public async Task<IActionResult> CountUsers()
        {
            var user = User;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var name = User.Identity?.Name;
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            var count = await _userRepository.CountUsers();
            return Ok(new
            {
                count = count,
                userId = userId,
                userEmail = userEmail,
                name = name,
                roles = roles
            });
        }
        [HttpPost("mfa-verified")]
        public IActionResult MfaVerify([FromQuery] string userId)
        {
            var otp = _cacheService.GetCache<string>($"verified-{userId}");
            var valid = _cacheService.IsCacheKeyValid($"verified-{userId}");
            if (valid)
            {
                return Ok("User current mfa verified!");
            }
            else
            {
                return BadRequest("User is not mfa verified!");
            }
        }
        [HttpPost("verify-password-before-change")]
        public async Task<IActionResult> VerifyPassword([FromQuery] string userId, [FromBody] string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var check = await _userManager.CheckPasswordAsync(user, password);
                if (check)
                {
                    return Ok("Password is correct!");
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
        [HttpPost("send-mail-update-user")]
        public async Task<IActionResult> SendMailUpdateUser([FromBody] string email)
        {
            try
            {
                var otp = StringExtensions.GenerateSecureRandomNumericCode();
                var emailBody = StringExtensions.GetOtpEmailTemplate(otp);

                await _emailSender.SendEmailAsync(email, "Your OTP Code", emailBody);
                _cacheService.SetCacheWithExpiration(email, otp, TimeSpan.FromMinutes(5));
                return Ok("Please open your mail to verify!");
            }
            catch (Exception)
            {
                return BadRequest("Send mail to verify failure!");
            }
        }
        [HttpPost("verify-update-user")]
        public  async Task<IActionResult> VerifyUpdateUser(OtpDto request)
        {
            try
            {
                var otp = _cacheService.GetCache<string>(request.Email);
                var valid = _cacheService.IsCacheKeyValid(request.Email);
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (valid)
                {
                    if (otp == request.Otp)
                    {
                        _cacheService.RemoveCache(request.Email);
                        _cacheService.SetCacheWithExpiration($"verified-{user.Id}", "verified", TimeSpan.FromMinutes(5));
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
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfile([FromRoute] string id, UserDto userDto)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User id is required");
            }
            var userFind = await _userRepository.FindByIdAsync(id);
            if(userFind == null)
            {
                return NotFound("User not found!");
            }
            userFind.FullName = userDto.FullName;
            userFind.Address = userDto.Address;
            var result = await _userRepository.UpdateUser(userFind);
            return Ok(result);
        }
        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? query, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var result = await _userRepository.GetAllUserAsync(query, pageNumber, pageSize);
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(result);
            return Ok(userDtos);
        }
        [HttpDelete("{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found!");
            }
           
            await _userRepository.DeleteAsync(user);
            return Ok();
        }
    }
}
