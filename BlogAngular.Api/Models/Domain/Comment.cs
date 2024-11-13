namespace BlogAngular.Api.Models.Domain
{
    public class Comment:BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public string? Subject { get; set; }
        public string Content { get; set; }
    }
}
