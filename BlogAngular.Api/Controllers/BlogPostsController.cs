using AutoMapper;
using BlogAngular.Api.Helpers;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Models.Dtos.AuthDtos;
using BlogAngular.Api.Models.Dtos.BlogDtos;
using BlogAngular.Api.Models.Dtos.CategoryDtos;
using BlogAngular.Api.Repositories.Implementation;
using BlogAngular.Api.Repositories.Interface;
using BlogAngular.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogAngular.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly CacheService _cacheService;
        private readonly UserManager<AppUser> _userManager;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository, 
            IMapper mapper, ILikeRepository likeRepository, IEmailSender emailSender, CacheService cacheService,
            UserManager<AppUser> userManager)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
            _likeRepository = likeRepository;
            _mapper = mapper;
            _emailSender = emailSender;
            _cacheService = cacheService;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? query, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var result = await _blogPostRepository.GetAllAsync(query, pageNumber, pageSize);
            var blogPostDtos = _mapper.Map<IEnumerable<BlogPostDto>>(result);
            return Ok(blogPostDtos);
        }
        [HttpGet("post-with-likes")]
        public async Task<IActionResult> GetAllPostWithLikesAsync([FromQuery] int? pageSize)
        {
            var result = await _blogPostRepository.GetAllPostWithLikesAsync(pageSize);
            if(result == null)
            {
                return NotFound();
            }
            var blogPostDtos = new List<PostLikeDto>();
            foreach (var post in result)
            {
                var pl = new PostLikeDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Slug = post.UrlHandle,
                    Likes = await _likeRepository.CountLikeByPost(post.Id)
                };
                blogPostDtos.Add(pl);
            }
            blogPostDtos = blogPostDtos.OrderByDescending(x => x.Likes).ToList();
            return Ok(blogPostDtos);
        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountPosts()
        {
            var count = await _blogPostRepository.CountPosts();
            return Ok(count);
        }
        [HttpGet]
        [Route("count-category-posts")]
        public async Task<IActionResult> CountCategoryPosts([FromQuery] Guid categoryId)
        {
            var count = await _blogPostRepository.CountCategoryPosts(categoryId);
            return Ok(count);
        }
        [HttpGet]
        [Route("count-user-posts")]
        public async Task<IActionResult> CountUserPosts([FromQuery] string authorId)
        {
            var count = await _blogPostRepository.CountPostsOfAuthor(authorId);
            return Ok(count);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync(CreateBlogPostRequestDto requestDto)
        {
            var urlHandle = StringExtensions.GenerateUrlHandle(requestDto.Title, "post");
            var blogPost = new BlogPost
            {
                Title = requestDto.Title,
                ShortDescription = requestDto.ShortDescription,
                Content = requestDto.Content,
                FeaturedImageUrl = requestDto.FeaturedImageUrl,
                UrlHandle = urlHandle,
                PublishedDate = requestDto.PublishedDate,
                AuthorId = requestDto.AuthorId,
                IsVisible = requestDto.IsVisible,
            };
            foreach (var cid in requestDto.Categories)
            {
                var existCategory = await _categoryRepository.FindByIdAsync(cid);
                if (existCategory != null)
                {
                    blogPost.Categories.Add(existCategory);
                }
            }
            var result = await _blogPostRepository.CreateAsync(blogPost);
            var blogPostDto = _mapper.Map<BlogPostDto>(result);
            blogPostDto.Categories = blogPost.Categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                UrlHandle = x.UrlHandle,
            }).ToList();
            return Ok(blogPostDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetByIdAsync([FromRoute]Guid id)
        {
            var blogPost = await _blogPostRepository.FindByIdAsync(id);
            if(blogPost == null)
            {
                return NotFound("Post not found!");
            }
            var blogPostDto = _mapper.Map<BlogPostDto>(blogPost);
            return Ok(blogPostDto);
        }
        [HttpGet("get-by-author")]
        public async Task<IActionResult> GetPostsByCategoryId([FromQuery]string authorId, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var result = await _blogPostRepository.GetByAuthorAsync(authorId,pageNumber, pageSize);
            var blogPostDtos = _mapper.Map<IEnumerable<BlogPostDto>>(result);
            return Ok(blogPostDtos);
        }
        [HttpGet("get-popular-posts")]
        public async Task<IActionResult> GetPupularPosts([FromQuery] int? postNumbers)
        {
            var result = await _blogPostRepository.GetPopularPosts(postNumbers);
            var blogPostDtos = _mapper.Map<IEnumerable<BlogPostDto>>(result);
            return Ok(blogPostDtos);
        }
        [HttpGet("get-by-category")]
        public async Task<IActionResult> GetPostsByCategoryId([FromQuery] Guid categoryId, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var result = await _blogPostRepository.GetByCategoryAsync(categoryId, pageNumber, pageSize);
            var blogPostDtos = _mapper.Map<IEnumerable<BlogPostDto>>(result);
            return Ok(blogPostDtos);
        }
        [HttpGet("get-posts-user-liked")]
        [Authorize]
        public async Task<IActionResult> GetPostsUserLiked([FromQuery] string userId, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var result = await _blogPostRepository.GetAllPostsUserLiked(userId, pageNumber, pageSize);
            var blogPostDtos = _mapper.Map<IEnumerable<BlogPostDto>>(result);
            return Ok(blogPostDtos);
        }
        [HttpGet("like-post/{url}")]
        [Authorize]
        public async Task<IActionResult> LikeBlogPost([FromQuery] string userId, [FromRoute] string url)
        {
            if(string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(url))
            {
                return BadRequest();
            }
            var blogPost = await _blogPostRepository.GetByUrlAsync(url);
            if(blogPost == null)
            {
                return NotFound("Blog not found");
            }
            var result = await _likeRepository.LikePost(userId, blogPost.Id);
            return Ok(result);
        }

        [HttpGet("get-like-post/{url}")]
        public async Task<IActionResult> GetLikeBlogPost([FromRoute] string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest();
            }
            var blogPost = await _blogPostRepository.GetByUrlAsync(url);
            if (blogPost == null)
            {
                return NotFound("Blog not found");
            }
            var result = await _likeRepository.CountLikeByPost(blogPost.Id);
            return Ok(result);
        }

        [HttpGet("check-liked/{url}")]
        public async Task<IActionResult> CheckLikedBlogPost([FromQuery] string userId,[FromRoute] string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest();
            }
            var blogPost = await _blogPostRepository.GetByUrlAsync(url);
            if (blogPost == null)
            {
                return NotFound("Blog not found");
            }
            var result = await _likeRepository.CheckLiked(userId, blogPost.Id);
            return Ok(result);
        }

        [HttpGet("{url}")]
        public async Task<IActionResult> GetByUrlAsync([FromRoute]string url)
        {
            var blogPost = await _blogPostRepository.GetByUrlAsync(url);
            if(blogPost == null)
            {
                return NotFound("Post not found!");
            }
            var blogPostDto = _mapper.Map<BlogPostDto>(blogPost);
            return Ok(blogPostDto);
        }
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateBlogPostRequestDto requestDto)
        {
            var post = await _blogPostRepository.FindByIdAsync(id);
            if(post == null)
            {
                return NotFound();
            }
            if(post.AuthorId != requestDto.AuthorId)
            {
                return Unauthorized();
            }
            var urlHandle = StringExtensions.GenerateUrlHandle(requestDto.Title, "post");
            var blogPost = new BlogPost
            {
                Id = id,
                Title = requestDto.Title,
                ShortDescription = requestDto.ShortDescription,
                Content = requestDto.Content,
                FeaturedImageUrl = requestDto.FeaturedImageUrl,
                UrlHandle = urlHandle,
                PublishedDate = requestDto.PublishedDate,
                AuthorId = requestDto.AuthorId,
                IsVisible = requestDto.IsVisible,
                UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")),
                Categories = new List<Category>()
            };
            foreach(var cid in requestDto.Categories)
            {
                var existCategory = await _categoryRepository.FindByIdAsync(cid);
                if (existCategory != null)
                {
                    blogPost.Categories.Add(existCategory);
                }
            }
            blogPost = await _blogPostRepository.UpdateAsync(blogPost);
            if(blogPost == null)
            {
                return NotFound();
            }
            var blogPostDto = _mapper.Map<BlogPostDto>(blogPost);
            return Ok(blogPostDto);
        }
        [HttpPost("user/post/send-mail-delete/{id}")]
        [Authorize]
        public async Task<IActionResult>DeleteBlogPost(Guid id, [FromQuery] string authorId)
        {
            var post = await _blogPostRepository.FindByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(authorId);

            if (post.AuthorId != authorId)
            {
                return Unauthorized();
            }
            var otp = StringExtensions.GenerateSecureRandomNumericCode();
            var emailBody = StringExtensions.GetOtpEmailTemplate(otp);
            if(user != null)
            {
                await _emailSender.SendEmailAsync(user.Email, "Your OTP Code", emailBody);
                _cacheService.SetCacheWithExpiration(id.ToString(), otp, TimeSpan.FromMinutes(5));
            }
           
            return Ok("Send Otp successfully!");
        }
        [HttpPost("user/delete-post-verify/{id}")]
        public IActionResult DeletePostVerify(Guid id, [FromQuery] string userId, [FromBody] string otpReq)
        {
            var otp = _cacheService.GetCache<string>(id.ToString());
            var valid = _cacheService.IsCacheKeyValid(id.ToString());

            if (valid)
            {
                if (otp == otpReq)
                {
                    _cacheService.RemoveCache(id.ToString());
                    _cacheService.SetCacheWithExpiration($"verified-{userId}", "verified", TimeSpan.FromMinutes(5));
                    return Ok("Verify delete post successfully!");
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
        [HttpDelete("user/post/{id}")]
        public async Task<IActionResult> DeleteBlogPostByUser(Guid id)
        {
            var post = await _blogPostRepository.FindByIdAsync(id);
            if (post == null)
            {
                return NotFound("Post not found");
            }
            await _blogPostRepository.DeleteAsync(post);
            return Ok("Delete successfully");
        }
        [HttpDelete("admin/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteBlogPostByAdmin(Guid id)
        {
            var post = await _blogPostRepository.FindByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            await _blogPostRepository.DeleteAsync(post);
            return Ok();
        }
    }
}
