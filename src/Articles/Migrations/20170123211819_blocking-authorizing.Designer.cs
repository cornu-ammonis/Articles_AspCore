using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Articles.Data;

namespace Articles.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170123211819_blocking-authorizing")]
    partial class blockingauthorizing
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Articles.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("UrlSlug");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Articles.Models.Core.BlogUser", b =>
                {
                    b.Property<int>("BlogUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BlogUserId1");

                    b.Property<int>("page_size");

                    b.Property<int>("subscribers_count");

                    b.Property<string>("user_name");

                    b.HasKey("BlogUserId");

                    b.HasIndex("BlogUserId1");

                    b.ToTable("BlogUser");
                });

            modelBuilder.Entity("Articles.Models.Core.CategoryBlogUser", b =>
                {
                    b.Property<int>("CategoryId");

                    b.Property<int>("BlogUserId");

                    b.HasKey("CategoryId", "BlogUserId");

                    b.HasIndex("BlogUserId");

                    b.ToTable("CategoryBlogUser");
                });

            modelBuilder.Entity("Articles.Models.Core.PostTag", b =>
                {
                    b.Property<int>("PostId");

                    b.Property<int>("TagId");

                    b.HasKey("PostId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("Articles.Models.Core.PostUserLike", b =>
                {
                    b.Property<int>("PostId");

                    b.Property<int>("BlogUserId");

                    b.HasKey("PostId", "BlogUserId");

                    b.HasIndex("BlogUserId");

                    b.ToTable("PostUserLikes");
                });

            modelBuilder.Entity("Articles.Models.Core.PostUserSave", b =>
                {
                    b.Property<int>("PostId");

                    b.Property<int>("BlogUserId");

                    b.HasKey("PostId", "BlogUserId");

                    b.HasIndex("BlogUserId");

                    b.ToTable("PostUserSaves");
                });

            modelBuilder.Entity("Articles.Models.Core.UserAuthorizesUser", b =>
                {
                    b.Property<int?>("authorizingUserId");

                    b.Property<int?>("userAuthorizedId");

                    b.HasKey("authorizingUserId", "userAuthorizedId");

                    b.HasIndex("userAuthorizedId");

                    b.ToTable("UserAuthorizesUsers");
                });

            modelBuilder.Entity("Articles.Models.Core.UserAuthorSubscribe", b =>
                {
                    b.Property<int?>("authorId");

                    b.Property<int?>("userId");

                    b.HasKey("authorId", "userId");

                    b.HasIndex("userId");

                    b.ToTable("UserAuthorSubscribes");
                });

            modelBuilder.Entity("Articles.Models.Core.UserBlocksUser", b =>
                {
                    b.Property<int?>("blockingUserId");

                    b.Property<int?>("userBlockedId");

                    b.HasKey("blockingUserId", "userBlockedId");

                    b.HasIndex("userBlockedId");

                    b.ToTable("UserBlocksUsers");
                });

            modelBuilder.Entity("Articles.Models.Link", b =>
                {
                    b.Property<int>("LinkId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("category");

                    b.Property<string>("title");

                    b.Property<string>("url");

                    b.HasKey("LinkId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Articles.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AuthorBlogUserId");

                    b.Property<int?>("CategoryId");

                    b.Property<string>("Description");

                    b.Property<int>("LikeCount");

                    b.Property<string>("Meta");

                    b.Property<DateTime?>("Modified");

                    b.Property<DateTime>("PostedOn");

                    b.Property<bool>("Published");

                    b.Property<string>("ShortDescription");

                    b.Property<string>("Title");

                    b.Property<string>("UrlSlug");

                    b.Property<int>("ViewCount");

                    b.HasKey("PostId");

                    b.HasIndex("AuthorBlogUserId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Articles.Models.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("UrlSlug");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Articles.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser");


                    b.ToTable("ApplicationUser");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Articles.Models.Core.BlogUser", b =>
                {
                    b.HasOne("Articles.Models.Core.BlogUser")
                        .WithMany("SubscribedAuthors")
                        .HasForeignKey("BlogUserId1");
                });

            modelBuilder.Entity("Articles.Models.Core.CategoryBlogUser", b =>
                {
                    b.HasOne("Articles.Models.Core.BlogUser", "BlogUser")
                        .WithMany("CategoryBlogUsers")
                        .HasForeignKey("BlogUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Articles.Models.Category", "Category")
                        .WithMany("CategoryBlogUsers")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Articles.Models.Core.PostTag", b =>
                {
                    b.HasOne("Articles.Models.Post", "Post")
                        .WithMany("PostTags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Articles.Models.Tag", "Tag")
                        .WithMany("PostTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Articles.Models.Core.PostUserLike", b =>
                {
                    b.HasOne("Articles.Models.Core.BlogUser", "BlogUser")
                        .WithMany("PostUserLikes")
                        .HasForeignKey("BlogUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Articles.Models.Post", "Post")
                        .WithMany("PostUserLikes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Articles.Models.Core.PostUserSave", b =>
                {
                    b.HasOne("Articles.Models.Core.BlogUser", "BlogUser")
                        .WithMany("PostUserSaves")
                        .HasForeignKey("BlogUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Articles.Models.Post", "Post")
                        .WithMany("PostUserSaves")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Articles.Models.Core.UserAuthorizesUser", b =>
                {
                    b.HasOne("Articles.Models.Core.BlogUser", "authorizingUser")
                        .WithMany("UsersThisUserAuthorizes")
                        .HasForeignKey("authorizingUserId");

                    b.HasOne("Articles.Models.Core.BlogUser", "userAuthorized")
                        .WithMany("UsersAuthorizingThisUser")
                        .HasForeignKey("userAuthorizedId");
                });

            modelBuilder.Entity("Articles.Models.Core.UserAuthorSubscribe", b =>
                {
                    b.HasOne("Articles.Models.Core.BlogUser", "author")
                        .WithMany("AuthorUserSubscribes")
                        .HasForeignKey("authorId");

                    b.HasOne("Articles.Models.Core.BlogUser", "user")
                        .WithMany("UserAuthorSubscribes")
                        .HasForeignKey("userId");
                });

            modelBuilder.Entity("Articles.Models.Core.UserBlocksUser", b =>
                {
                    b.HasOne("Articles.Models.Core.BlogUser", "blockingUser")
                        .WithMany("UsersThisUserBlocks")
                        .HasForeignKey("blockingUserId");

                    b.HasOne("Articles.Models.Core.BlogUser", "userBlocked")
                        .WithMany("UsersBlockingThisUser")
                        .HasForeignKey("userBlockedId");
                });

            modelBuilder.Entity("Articles.Models.Post", b =>
                {
                    b.HasOne("Articles.Models.Core.BlogUser", "Author")
                        .WithMany("AuthoredPosts")
                        .HasForeignKey("AuthorBlogUserId");

                    b.HasOne("Articles.Models.Category", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
