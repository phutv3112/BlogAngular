using BlogAngular.Api.Models.Domain;

namespace BlogAngular.Api.Models.Dtos.CommentDtos
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string AuthorName { get; set; }
        public Guid BlogPostId { get; set; }
        public string Content { get; set; }
        public Guid? ParentId { get; set; }
        public ICollection<CommentDto>? Replies { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
