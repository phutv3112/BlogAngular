namespace BlogAngular.Api.Models.Domain
{
    public class BlogImage : BaseEntity
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

    }
}
