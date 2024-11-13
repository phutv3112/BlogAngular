namespace BlogAngular.Api.Models.Domain
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string UrlHandle { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}
