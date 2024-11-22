namespace BlogAngular.Api.Repositories.Interface
{
    public interface ILikeRepository
    {
        Task<int> LikePost(string userId, Guid postId);
        Task<int> CountLikeByPost(Guid postId);
        Task<bool> CheckLiked(string userId, Guid postId);
    }
}
