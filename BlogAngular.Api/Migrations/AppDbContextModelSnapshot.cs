﻿// <auto-generated />
using System;
using BlogAngular.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlogAngular.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("EncryptedPrivateKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "11872d42-f137-430d-a396-46498fc4e3a7",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "11872d42-f137-430d-a396-46498fc4e3a7",
                            CreatedDate = new DateTime(2024, 11, 22, 13, 44, 10, 518, DateTimeKind.Utc).AddTicks(8911),
                            Email = "admin@gmail.com",
                            EmailConfirmed = false,
                            EncryptedPrivateKey = "4XA7UhPJ5BSdRnhiOamoRA==:20zVgoDO+CF+NTfs8jvKyFltIsaty+/Oa7tcTsa3z+nBSkXc9VIPw59Fk9j1p9x8c7SNdo3PVW+jxU+C3JmwA1OoZDT4j19iMRxmLRPor275QIK0NbVdZSZ80Ho6E9A0k0j+Ah5CyTY/axoKxv83+dZ4SQOUm/cctniNB0XmYNpZrQpdwOvXgBGlpVaPw8K061W/YWBH9CAZE4nVv1CyVUIa1OYgmZmjX9yAK4RrV1EmSB9uK9amlFQ8INKkzwPtirDAYr+sd88Uqbb8NICAVJtPXcLXOM7NzyqfXM6jfOo3XIBAj0p6UCSyY+Brhs4TfnAbvxdYb5b7G+axhowAymZsAXOL/xA49PKfk1ouaDGSZL1a+3xPErwdo7gKod5fkyVX+Gk2SMZCgyIJrjTZ9YLR0JKBkGu51wshwvpYUE2crR5PV/ovkpDkykQYtF70WHiw4st95Gu1wod/94g9Llb3pufFxuZtUXtkrmyrHjb9A/KZnNJlDWCBycRGGovWT+JWoUKBhRAdevfR2TwYyGbyYojkjgHLcqMtma2Wk2bJVAY+rEqvAs+1sKqYtyuzORFV1Ms9uKMT2kwUSKnbo/CQi7C2YmzmujpRw4t8zhcHElFFjPK2h//vcdqCP3DwG2qP+g1manSXGKQU3RY9RMmXNqU6orUhT9y9TxjLIKg/5h0mG6Cp7Q6sMf7B4MXMeUX/zaSpQG+5Fcca39AdasMj8BEAvUWeODlGrtq623zX5CO4CNemgi80onxLUioFZiIxBqwbWYLZqLJQeZvXvVjirdA2SkLTq+cjzvNY3hlnrdjeOdOuA1w21hNOybmfURVTIXDDEr4P0Qy5BJguvxPn0gfUuLENoZzZChS1dwf0qnDqnlGfLhLd1S+nGtvu6PcLfqzOjpCAvfzqATlAoIqysWHK4HcFENVCF3xppWTEOHnygnMALYa13BVBcO3RhpetzPZzSHjrkaJJuRfSl7UYYfZmYbcRSXyUZ1grOUqDY0DbWGjfdfzEvDfrHniKPPjBk6hLlFu9D2aqMeaeBhRYjFUqRsox0e/oVAM38yl2Kwwdy004pMpgU98Gn2WgHp4olypW4GBjx3LPslGwbkF3Zyx8gcLPXQ8Gpig7Hfj3IJKUb/zBbaTByPTjFpEEjgNVhhJJ+gKlAfB0jJfcOKFhX37wKbE6l5rJaDwKRi8DvNB0kBzpKX/uoqysmUp6wlNN7XU+i/CsObmfv0rhsPYo5Qm6L2puHpsBtpD8XUK6QUMYiMsuFGaYycOKPEuF1I/4+bZzh2wDoXqYB+pVT+KASkSx92CnM5nZiLEx7X5NaD2AERfF59pNL0fgdLZAdgCUMeReUGlMEAIn3rfqEBHjhJnufBJaIXFxdob/AuOpQGhg8wOQre7fi0Kc036BgaT4V70H6jdOnZhzxDdvs8n63E2TIt1g/cUW6f5Ur0ypOirORU74y5HKt2eNDG9KMlLkGnPAkH19YWlRonabffzMNxi4vV69TrcR1jfWWrXhvzz9VIFFJ5GWKFKw8yj50CTaDTqawOQhzZyP1j4cwm32hkW/sbocG3q87NdIF3QJTJhgb254KrN6CYh/CBFPbfL+RC7u7McfVPT6bikgkZFx/Ff467wiMRzl0DO8okCIN0ttnhVZmZaJpRG9WuLW4vnAkDEr5R2Qj6zzFdTuYV9l2jFfssX0pi9rdLPc59Nz86rhl4upWwKQhnzDnco8PhsSKQi3Ue/mFlniC8ukKh/JZ2n0IycBWb+MBrsUV9ZEJMQGKeRnSksOQw9nweuK9oQe7m1eXqjkMByMylo7u6AGZmXYx6Aanldxzf10AN3wBQq7+ElTgvqhLtcqSan0qTXyPP3z76ufv5Yza+8qQvY3ADI4frM4G4bUbW8y8XWkB34zoF1fEEPNEfz1RageJFqE4nMipwZwuK47nPXqUedFUTHFBnzzuQF723Mugotsb7wrc+zEZTejQbA75M0GIQ5ZXWzn/cOGPMN/CZrSuZ0VavxqsJ64P7dWqKR2wkaoRbSTDNjC7CSAkDyqbLixrs1mJybUt8AtxE7cH/P0CIneP7GpxfkS7+R63z4QbMHzv4wAIW7i79VoS+CRDNkOBfPJBQYbTr832szxRBfVm1hX379v8LoG8Eo3VB/JIISw7WxwY/5iVOURlbO3kB8KRF0SVzEpxQlKyppR2Xtx80cZl+ZT+StlGBiE/ChbwTxH/O7hdDDmoSlUqtXs/MbH",
                            FullName = "Admin",
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@GMAIL.COM",
                            NormalizedUserName = "ADMIN@GMAIL.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEBwuTgKrWG3Rl9Tc1aN+ujkXNuLABx7HJCQlWc75Jl60ved4L/UVkeOEzL9QJLv8Fg==",
                            PhoneNumberConfirmed = false,
                            PublicKey = "<RSAKeyValue><Modulus>3mos4NdjIzOZXV/6TvwQX1gbmmN5v3VIxxa4F1rjyYYG1LkL4FHIolnXaWuL7N/FF8lMja0cQNvux+qW+GIpuBj12d8zWdh92IXqH2L25xKKz7ch+pAZifpxisfM/yxlb/kQiVIYd/wqihIvHJOYcWrm2d+yXZ/Rvk4ka/lZHRTJawkksWG5dc/LNiYYVaWRHoBi9M/ndrnivcYg1Tusxe7+BfJmORll93XCJ1qNpmhhA7tvrRGKYKwXWKOXZhnidQipu0kPV0uXb0lAQz67+60RcztLvci2pX9768PC0Cj3tdHdEVacOUU0L0xr24dpa6McXTDeUJzEn1jurkTTMQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>",
                            SecurityStamp = "11872d42-f137-430d-a396-46498fc4e3a7",
                            TwoFactorEnabled = false,
                            UpdatedDate = new DateTime(2024, 11, 22, 13, 44, 10, 518, DateTimeKind.Utc).AddTicks(8917),
                            UserName = "admin@gmail.com"
                        });
                });

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.BlogImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BlogImages");
                });

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.BlogPost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FeaturedImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UrlHandle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BlogPosts");
                });

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UrlHandle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BlogPostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BlogPostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.PostLike", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsUnLiked")
                        .HasColumnType("bit");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("PostLikes");
                });

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.PostTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTags");
                });

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("BlogPostCategory", b =>
                {
                    b.Property<Guid>("BlogPostsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoriesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BlogPostsId", "CategoriesId");

                    b.HasIndex("CategoriesId");

                    b.ToTable("BlogPostCategory");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1e4be785-839a-4a52-a3b9-455f3e289c44",
                            ConcurrencyStamp = "1e4be785-839a-4a52-a3b9-455f3e289c44",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "1e4be785-839a-4a52-a3b9-455f3e289c45",
                            ConcurrencyStamp = "1e4be785-839a-4a52-a3b9-455f3e289c45",
                            Name = "Reader",
                            NormalizedName = "READER"
                        },
                        new
                        {
                            Id = "1e4be785-839a-4a52-a3b9-455f3e289c46",
                            ConcurrencyStamp = "1e4be785-839a-4a52-a3b9-455f3e289c46",
                            Name = "Writer",
                            NormalizedName = "WRITER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "11872d42-f137-430d-a396-46498fc4e3a7",
                            RoleId = "1e4be785-839a-4a52-a3b9-455f3e289c44"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.Comment", b =>
                {
                    b.HasOne("BlogAngular.Api.Models.Domain.BlogPost", "BlogPost")
                        .WithMany("Comments")
                        .HasForeignKey("BlogPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogAngular.Api.Models.Domain.AppUser", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BlogPost");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.PostLike", b =>
                {
                    b.HasOne("BlogAngular.Api.Models.Domain.BlogPost", "Post")
                        .WithMany("PostLikes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogAngular.Api.Models.Domain.AppUser", "User")
                        .WithMany("PostLikes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.PostTag", b =>
                {
                    b.HasOne("BlogAngular.Api.Models.Domain.BlogPost", "Post")
                        .WithMany("PostTags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogAngular.Api.Models.Domain.Tag", "Tag")
                        .WithMany("PostTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("BlogPostCategory", b =>
                {
                    b.HasOne("BlogAngular.Api.Models.Domain.BlogPost", null)
                        .WithMany()
                        .HasForeignKey("BlogPostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogAngular.Api.Models.Domain.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BlogAngular.Api.Models.Domain.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BlogAngular.Api.Models.Domain.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogAngular.Api.Models.Domain.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BlogAngular.Api.Models.Domain.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.AppUser", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("PostLikes");
                });

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.BlogPost", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("PostLikes");

                    b.Navigation("PostTags");
                });

            modelBuilder.Entity("BlogAngular.Api.Models.Domain.Tag", b =>
                {
                    b.Navigation("PostTags");
                });
#pragma warning restore 612, 618
        }
    }
}
