using AutoMapper;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Models.Dtos.BlogDtos;
using BlogAngular.Api.Models.Dtos.CategoryDtos;
using BlogAngular.Api.Models.Dtos.CommentDtos;
using BlogAngular.Api.Repositories.Implementation;
using BlogAngular.Api.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace BlogAngular.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IBlogPostRepository _postRepository;

        public CommentController(ICommentRepository commentRepository, IMapper mapper, IBlogPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _postRepository = postRepository;
        }
        [HttpGet("{postUrl}")]
        public async Task<IActionResult> GetCommentsOfPost([FromRoute] string postUrl)
        {
            var post = await _postRepository.GetByUrlAsync(postUrl);
            if (post == null)
            {
                return NotFound();
            }
            var comments = await _commentRepository.GetCommentsOfPost(post.Id);
            var commentDtos = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return Ok(commentDtos);
        }
        [HttpGet("admin/post/{postId}")]
        public async Task<IActionResult> GetCommentsOfPost([FromRoute] Guid postId, [FromQuery] string? query, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var post = await _postRepository.FindByIdAsync(postId);
            if (post == null)
            {
                return NotFound();
            }
            var comments = await _commentRepository.GetAllCommentsByPost(post.Id,query, pageNumber, pageSize);
            var commentDtos = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return Ok(commentDtos);
        }

        [HttpGet]
        [Route("count/{postId}")]
        public async Task<IActionResult> CountCommentsOfPost([FromRoute] Guid postId)
        {
            var post = await _postRepository.FindByIdAsync(postId);
            if (post == null)
            {
                return NotFound();
            }
            var count = await _commentRepository.CountCommentsOfPost(post.Id);
            return Ok(count);
        }

        [HttpGet]
        [Route("get-subcomments")]
        public async Task<IActionResult> GetSubcomments([FromQuery] Guid postId, [FromQuery] Guid parentId)
        {
            var post = await _postRepository.FindByIdAsync(postId);
            if (post == null)
            {
                return NotFound();
            }
            var comments = await _commentRepository.GetSubcomments(post.Id, parentId);
            var commentDtos = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return Ok(commentDtos);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CommentPost(CreateCommentRequestDto requestDto)
        {
            var post = await _postRepository.FindByIdAsync(requestDto.PostId);
            if (post == null)
            {
                return NotFound();
            }
           
            var comment = new Comment
            {
                UserId = requestDto.UserId,
                BlogPostId = post.Id,
                Content = requestDto.Content,
                ParentId = requestDto.ParentId
            };
            var result = await _commentRepository.CommentPost(comment);
            return Ok(result);
        }
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(Guid id, string content)
        {
            var result = await _commentRepository.Update(id, content);
            var commentDto = _mapper.Map<CommentDto>(result);
            return Ok(commentDto);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCommentWithReplies(Guid id)
        {
            var comment = await _commentRepository.FindById(id);
            if (comment == null)
            {
                return NotFound();
            }
            var result = await _commentRepository.Delete(id);
            return Ok(result);
        }
    }
}
