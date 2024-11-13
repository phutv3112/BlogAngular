using AutoMapper;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Models.Dtos.BlogDtos;
using BlogAngular.Api.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAngular.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public ImagesController(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetAll();
            var imageDtos = _mapper.Map<IEnumerable<BlogImageDto>>(images);
            return Ok(imageDtos);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    Title = title,
                    FileName = fileName,
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                var result = await _imageRepository.Upload(file, blogImage);
                var blogImageDto = _mapper.Map<BlogImageDto>(result);
                return Ok(blogImageDto);
            }
            return BadRequest(ModelState);  
        }
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file");
            }
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format!");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size must be less than 10MB");
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            var image = await _imageRepository.GetById(id);
            if (image == null)
            {
                return NotFound();
            }
            await _imageRepository.Delete(image);
            return NoContent();
        }
    }
}
