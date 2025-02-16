namespace BlogAngular.Api.Models.Dtos.CommentDtos
{
    public class CreateCommentRequestDto
    {
        public string Content { get; set; }
        public Guid PostId { get; set; }
        public string UserId { get; set; }
        public Guid? ParentId { get; set; }
    }
}
