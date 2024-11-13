using AutoMapper;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Models.Dtos.BlogDtos;
using BlogAngular.Api.Models.Dtos.CategoryDtos;
using BlogAngular.Api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAngular.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var result = await _blogPostRepository.GetAllAsync(pageNumber, pageSize);
            var blogPostDtos = _mapper.Map<IEnumerable<BlogPostDto>>(result);
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

        [HttpPost]
        [Authorize(Roles ="Admin, Writer")]
        public async Task<IActionResult> CreateAsync(CreateBlogPostRequestDto requestDto)
        {
            var blogPost = new BlogPost
            {
                Title = requestDto.Title,
                ShortDescription = requestDto.ShortDescription,
                Content = requestDto.Content,
                FeaturedImageUrl = requestDto.FeaturedImageUrl,
                UrlHandle = requestDto.UrlHandle,
                PublishedDate = requestDto.PublishedDate,
                Author = requestDto.Author,
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
            var blogPostDto = _mapper.Map<BlogPostDto>(blogPost);
            return Ok(blogPostDto);
        }
        [HttpGet("get-by-category")]
        public async Task<IActionResult> GetPostsByCategoryId([FromQuery]Guid categoryId, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var result = await _blogPostRepository.GetByCategoryAsync(categoryId,pageNumber, pageSize);
            var blogPostDtos = _mapper.Map<IEnumerable<BlogPostDto>>(result);
            return Ok(blogPostDtos);
        }
        [HttpGet("{url}")]
        [Authorize]
        public async Task<IActionResult> GetByUrlAsync([FromRoute]string url)
        {
            var blogPost = await _blogPostRepository.GetByUrlAsync(url);
            var blogPostDto = _mapper.Map<BlogPostDto>(blogPost);
            return Ok(blogPostDto);
        }
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin, Writer")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateBlogPostRequestDto requestDto)
        {
            var blogPost = new BlogPost
            {
                Id = id,
                Title = requestDto.Title,
                ShortDescription = requestDto.ShortDescription,
                Content = requestDto.Content,
                FeaturedImageUrl = requestDto.FeaturedImageUrl,
                UrlHandle = requestDto.UrlHandle,
                PublishedDate = requestDto.PublishedDate,
                Author = requestDto.Author,
                IsVisible = requestDto.IsVisible
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
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Writer")]
        public async Task<IActionResult>DeleteBlogPost(Guid id)
        {
            var blogPost = await _blogPostRepository.FindByIdAsync(id);
            if(blogPost == null)
            {
                return NotFound();
            }
            await _blogPostRepository.DeleteAsync(blogPost);
            return Ok();
        }
    }
}
