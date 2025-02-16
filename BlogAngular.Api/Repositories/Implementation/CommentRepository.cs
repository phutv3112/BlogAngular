using BlogAngular.Api.Data;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace BlogAngular.Api.Repositories.Implementation
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> CommentPost(Comment comment)
        {
            if(comment.ParentId != Guid.Empty)
            {
                var parentComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == comment.ParentId);
                if(parentComment == null)
                {
                    comment.ParentId = null;
                }
            }
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return await CountCommentsOfPost(comment.BlogPostId);
        }

        public async Task<int> CountCommentsOfPost(Guid postId)
        {
            var comments = await _context.Comments.Where(c => c.BlogPostId == postId).ToListAsync();
            return comments.Count();
        }

        public async Task<int> Delete(Guid id)
        {
            // Lấy comment cần xóa
            var comment = await _context.Comments
                .Include(c => c.Replies) // Bao gồm các comment con
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return 0; // Không tìm thấy comment
            }

            // Gọi hàm đệ quy để xóa các comment con
            DeleteReplies(comment);

            // Xóa comment cha
            _context.Comments.Remove(comment);

            // Lưu thay đổi vào DB
            return await _context.SaveChangesAsync();
        }

        // Hàm đệ quy để xóa các comment con
        private void DeleteReplies(Comment comment)
        {
            if (comment.Replies != null && comment.Replies.Any())
            {
                foreach (var reply in comment.Replies)
                {
                    // Gọi đệ quy để xóa các comment con của reply
                    DeleteReplies(reply);

                    // Xóa reply khỏi DbSet
                    _context.Comments.Remove(reply);
                }
            }
        }

        public async Task<Comment?> FindById(Guid id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            return comment;
        }
        public async Task<IEnumerable<Comment>> GetAllCommentsByPost(Guid postId, string? query = null, int? pageNumber = 1, int? pageSize = 5)
        {
            var comments = _context.Comments.AsQueryable();

            comments = comments.Include(b => b.User).OrderByDescending(d => d.UpdatedDate);
            if (!string.IsNullOrEmpty(query)){

                comments = comments.Where(c => c.Content.Contains(query) || c.User.UserName.Contains(query));
            }
            comments = comments.Where(b => b.BlogPostId == postId);
            // Pagination
            if (pageNumber <= 1)
            {
                pageNumber = 1;
            }
            var skip = (pageNumber - 1) * pageSize;
            comments = comments.Skip(skip ?? 0).Take(pageSize ?? 3);
            return await comments.ToListAsync();
        }
        public async Task<IEnumerable<Comment>> GetCommentsOfPost(Guid postId)
        {
            var comments = await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Replies)
                .Where(c => c.BlogPostId == postId).ToListAsync();

            // Lọc ra các comment gốc (không có parentId hoặc parentId = null)
            var rootComments = comments.Where(c => c.ParentId == null).ToList();

            // Duyệt qua từng comment gốc và thêm các comment con (nếu có)
            foreach (var rootComment in rootComments)
            {
                rootComment.Replies = comments.Where(c => c.ParentId == rootComment.Id).ToList();
            }

            // Kết quả là danh sách các comment gốc cùng với comment con lồng theo
            var result = rootComments;

            return result;
        }

        
        public async Task<IEnumerable<Comment>> GetSubcomments(Guid postId, Guid parentId)
        {
            if (postId == Guid.Empty || parentId == Guid.Empty)
            {
                return Enumerable.Empty<Comment>();
            }

            // Lấy tất cả comments của bài viết
            var allComments = await _context.Comments
                .Include(c => c.User)
                .Where(c => c.BlogPostId == postId)
                .ToListAsync();

            // Lọc các comment có parentId là parentId truyền vào
            var parentComment = allComments
                .Where(c => c.Id == parentId)
                .FirstOrDefault();

            if (parentComment == null)
            {
                return Enumerable.Empty<Comment>();
            }

            // Lọc các comment con trực tiếp từ parentComment và áp dụng đệ quy chỉ cho các comment này
            var subComments = GetRepliesWithSubcomments(allComments, parentComment);

            return subComments;
        }

        // Hàm đệ quy để lấy replies của comment và các replies của các replies (subcomments)
        private List<Comment> GetRepliesWithSubcomments(IEnumerable<Comment> allComments, Comment parent)
        {
            var directReplies = allComments
                .Where(c => c.ParentId == parent.Id) // Lấy các comment con trực tiếp
                .ToList();

            // Tạo danh sách chứa các comment con (cả trực tiếp và đệ quy)
            var allReplies = new List<Comment>();

            foreach (var reply in directReplies)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == reply.UserId);
                // Gọi đệ quy để lấy các subcomment (cấp con nhỏ hơn)
                var repliesWithSubcomments = new Comment
                {
                    Id = reply.Id,
                    UserId = reply.UserId,
                    User = user,
                    BlogPostId = reply.BlogPostId,
                    Content = reply.Content,
                    ParentId = reply.ParentId,
                    Replies = GetRepliesWithSubcomments(allComments, reply), // Đệ quy lấy replies của replies
                    UpdatedDate = reply.UpdatedDate
                };

                // Thêm reply vào danh sách
                allReplies.Add(repliesWithSubcomments);
            }

            // Trả về các comment con trực tiếp và các replies của chúng
            return allReplies;
        }



        public async Task<Comment?> Update(Guid id, string content)
        {
            var existComment = await _context.Comments.FirstOrDefaultAsync(b => b.Id == id);
            if (existComment != null)
            {
                existComment.UpdatedDate = DateTime.Now;
                existComment.Content = content;
                await _context.SaveChangesAsync();
                return existComment;
            }
            return null;
        }
    }
}
