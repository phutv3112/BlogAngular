using BlogAngular.Api.Models.Domain;

namespace BlogAngular.Api.Repositories.Interface
{
    public interface ICommentRepository
    {
        Task<int> CommentPost(Comment comment);
        Task<int> CountCommentsOfPost(Guid postId);
        Task<IEnumerable<Comment>> GetCommentsOfPost(Guid postId);
        Task<IEnumerable<Comment>> GetAllCommentsByPost(Guid postId, string? query = null, int? pageNumber = 1, int? pageSize = 5);
        Task<Comment?> FindById(Guid id);
        Task<Comment?> Update(Guid id, string content);
        Task<int> Delete(Guid id);
        Task<IEnumerable<Comment>> GetSubcomments(Guid postId, Guid parentId);
    }
}
