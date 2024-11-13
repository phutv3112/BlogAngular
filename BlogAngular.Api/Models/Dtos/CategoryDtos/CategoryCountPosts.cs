namespace BlogAngular.Api.Models.Dtos.CategoryDtos
{
    public class CategoryCountPosts
    {
        public Guid CateId { get; set; }
        public string Name { get; set; }
        public int CountPosts { get; set; }
    }
}
