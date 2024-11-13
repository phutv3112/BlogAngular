using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BlogAngular.Api.Models.Domain
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
        public string PublicKey { get; set; } = default!;
        public string EncryptedPrivateKey { get; set; } = default!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
    }
}
