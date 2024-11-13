namespace BlogAngular.Api.Models.Domain
{
    public class PostLike:BaseEntity
    {
        public Guid PostId { get; set; }
        public BlogPost Post { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
