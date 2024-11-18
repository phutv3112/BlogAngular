using BlogAngular.Api.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogAngular.Api.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        private readonly IConfiguration _configuration;
        public AppDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var entityTypes = builder.Model.GetEntityTypes();
            foreach (var entityType in entityTypes)
            {
                var tableName = entityType.GetTableName();
                if(tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            var adminRoleId = "1e4be785-839a-4a52-a3b9-455f3e289c44";
            var readerRoleId = "1e4be785-839a-4a52-a3b9-455f3e289c45";
            var writerRoleId = "1e4be785-839a-4a52-a3b9-455f3e289c46";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            var (publicKey, privateKey) = EncryptHelper.GenerateRsaKeyPair();
            var encryptedPrivateKey = EncryptHelper.EncryptData(privateKey, _configuration);

            var adminUserId = "11872d42-f137-430d-a396-46498fc4e3a7";
            var admin = new AppUser
            {
                Id = adminUserId,
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                FullName = "Admin",
                NormalizedEmail = "admin@gmail.com".ToUpper(),
                NormalizedUserName = "admin@gmail.com".ToUpper(),
                ConcurrencyStamp = adminUserId,
                SecurityStamp = adminUserId,
                PasswordHash = new PasswordHasher<AppUser>().HashPassword(null, "Admin@12345"),
                EncryptedPrivateKey = encryptedPrivateKey,
                PublicKey = publicKey
            };

            builder.Entity<AppUser>().HasData(admin);

            var adminRoles = new List<IdentityUserRole<string>>
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = adminRoleId,
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

    }
}
