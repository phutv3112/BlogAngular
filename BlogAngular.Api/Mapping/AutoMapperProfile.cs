using AutoMapper;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Models.Dtos.BlogDtos;
using BlogAngular.Api.Models.Dtos.CategoryDtos;

namespace BlogAngular.Api.Mapping
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateCategoryRequestDto, Category>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<BlogPostDto, BlogPost>().ReverseMap();
            CreateMap<CreateBlogPostRequestDto, BlogPost>().ReverseMap();
            CreateMap<BlogImageDto, BlogImage>().ReverseMap();
        }
    }
}
