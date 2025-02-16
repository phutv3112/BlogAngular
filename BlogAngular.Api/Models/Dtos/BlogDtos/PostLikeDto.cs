namespace BlogAngular.Api.Models.Dtos.BlogDtos
{
    public class PostLikeDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public int Likes { get; set; }
    }
}
