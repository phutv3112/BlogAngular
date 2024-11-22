using BlogAngular.Api.Data;
using BlogAngular.Api.Models.Domain;
using BlogAngular.Api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BlogAngular.Api.Repositories.Implementation
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _context;
        public LikeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> CountLikeByPost(Guid postId)
        {
            var likes = await _context.PostLikes.Where(l => l.PostId == postId && !l.IsUnLiked).ToListAsync();
            return likes.Count();
        }

        public async Task<int> LikePost(string userId, Guid postId)
        {
            var foundLike = await _context.PostLikes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            if (foundLike != null)
            {
                foundLike.IsUnLiked = !foundLike.IsUnLiked;
            }
            else
            {
                var like = new PostLike
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    PostId = postId,
                    IsUnLiked = false,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                await _context.PostLikes.AddAsync(like);
            }
            await _context.SaveChangesAsync();
            
            return await CountLikeByPost(postId);
        }
        public async Task<bool> CheckLiked(string userId, Guid postId)
        {
            var like = await _context.PostLikes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            return like != null && !like.IsUnLiked;
        }
    }
}
