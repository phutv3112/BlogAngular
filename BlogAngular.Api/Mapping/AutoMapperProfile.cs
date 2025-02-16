using AutoMapper;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Models.Dtos.BlogDtos;
using BlogAngular.Api.Models.Dtos.CategoryDtos;
using BlogAngular.Api.Models.Dtos.CommentDtos;
using BlogAngular.Api.Models.Dtos.UserDtos;

namespace BlogAngular.Api.Mapping
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateCategoryRequestDto, Category>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<BlogPost, BlogPostDto>()
                .ForMember(destinationMember => destinationMember.AuthorName, 
                    memberOptions => memberOptions.MapFrom(sourceMember => sourceMember.Author.UserName))
                .ReverseMap();
            CreateMap<CreateBlogPostRequestDto, BlogPost>().ReverseMap();
            CreateMap<BlogImageDto, BlogImage>().ReverseMap();
            CreateMap<Comment, CommentDto>()
                .ForMember(destinationMember => destinationMember.AuthorName,
                    memberOptions => memberOptions.MapFrom(sourceMember => sourceMember.User.UserName))
                .ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
        }
    }
}
