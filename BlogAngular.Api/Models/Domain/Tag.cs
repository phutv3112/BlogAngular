namespace BlogAngular.Api.Models.Domain
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
