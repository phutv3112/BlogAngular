namespace BlogAngular.Api.Models.Domain
{
    public class PostTag:BaseEntity
    {
        public Guid PostId { get; set; }
        public BlogPost Post { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
