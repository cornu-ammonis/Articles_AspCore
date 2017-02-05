using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Articles.Models;
using Articles.Models.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using Articles.Models.MessageViewModels;

namespace Articles.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }


        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet <Tag> Tags { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<BlogUser> BlogUser { get; set; }
        public DbSet<CategoryBlogUser> CategoryBlogUser { get; set; }
        public DbSet<PostUserSave> PostUserSaves { get; set; }
        public DbSet<PostUserLike> PostUserLikes { get; set; }
       public DbSet<UserAuthorSubscribe> UserAuthorSubscribes { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<UserBlocksUser> UserBlocksUsers { get; set; }
        public DbSet<UserAuthorizesUser> UserAuthorizesUsers { get; set; }
        public DbSet<Message> Messages { get; set; }

        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<PostTag>()
                .HasKey(t => new { t.PostId, t.TagId });

            builder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            builder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);

            builder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts);

            builder.Entity<CategoryBlogUser>()
                .HasKey(c => new { c.CategoryId, c.BlogUserId });

            builder.Entity<CategoryBlogUser>()
                .HasOne(cb => cb.Category)
                .WithMany(cb => cb.CategoryBlogUsers)
                .HasForeignKey(cb => cb.CategoryId);

            builder.Entity<CategoryBlogUser>()
                .HasOne(cb => cb.BlogUser)
                .WithMany(b => b.CategoryBlogUsers)
                .HasForeignKey(cb => cb.BlogUserId);

            builder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithMany(bu => bu.AuthoredPosts);

            builder.Entity<PostUserSave>()
                .HasKey(pu => new { pu.PostId, pu.BlogUserId });

            builder.Entity<PostUserSave>()
                .HasOne(pu => pu.Post)
                .WithMany(p => p.PostUserSaves)
                .HasForeignKey(pu => pu.PostId);

            builder.Entity<PostUserSave>()
                .HasOne(pu => pu.BlogUser)
                .WithMany(bu => bu.PostUserSaves)
                .HasForeignKey(pu => pu.BlogUserId);

            builder.Entity<PostUserLike>()
                .HasKey(pu => new { pu.PostId, pu.BlogUserId });

            builder.Entity<PostUserLike>()
                .HasOne(pu => pu.Post)
                .WithMany(p => p.PostUserLikes)
                .HasForeignKey(pu => pu.PostId);

            builder.Entity<PostUserLike>()
                 .HasOne(pu => pu.BlogUser)
                .WithMany(bu => bu.PostUserLikes)
                .HasForeignKey(pu => pu.BlogUserId);

            /*
            builder.Entity<UserAuthorSubscribe>()
                .HasKey(ua => new { ua.authorId, ua.userId });

            builder.Entity<UserAuthorSubscribe>()
                .HasOne(ua => ua.user)
                .WithMany(ua => ua.UserAuthorSubscribes)
                .HasForeignKey(ua => ua.userId)
                 .Metadata.DeleteBehavior = DeleteBehavior.Restrict; 

            builder.Entity<UserAuthorSubscribe>()
                .HasOne(ua => ua.author)
                .WithMany(ua => ua.AuthorUserSubscribes)
                .HasForeignKey(ua => ua.authorId)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;*/

            builder.Entity<UserBlocksUser>()
                .HasKey(ub => new { ub.blockingUserId, ub.userBlockedId });

            builder.Entity<UserBlocksUser>()
                .HasOne(ub => ub.blockingUser)
                .WithMany(u => u.UsersThisUserBlocks)
                .HasForeignKey(ub => ub.blockingUserId)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            builder.Entity<UserBlocksUser>()
                .HasOne(ub => ub.userBlocked)
                .WithMany(u => u.UsersBlockingThisUser)
                .HasForeignKey(ub => ub.userBlockedId)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;


            builder.Entity<UserAuthorizesUser>()
                .HasKey(ua => new { ua.authorizingUserId, ua.userAuthorizedId });

            builder.Entity<UserAuthorizesUser>()
                .HasOne(ua => ua.authorizingUser)
                .WithMany(u => u.UsersThisUserAuthorizes)
                .HasForeignKey(ua => ua.authorizingUserId)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            builder.Entity<UserAuthorizesUser>()
                .HasOne(ua => ua.userAuthorized)
                .WithMany(u => u.UsersAuthorizingThisUser)
                .HasForeignKey(ua => ua.userAuthorizedId)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            builder.Entity<UserAuthorSubscribe>()
                .HasKey(ua => new { ua.subscribingUserId, ua.userSubscribedId });

            builder.Entity<UserAuthorSubscribe>()
                .HasOne(ua => ua.subscribingUser)
                .WithMany(u => u.UsersThisUserSubscribesTo)
                .HasForeignKey(ua => ua.subscribingUserId)
                 .Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            builder.Entity<UserAuthorSubscribe>()
                .HasOne(ua => ua.userSubscribed)
                .WithMany(u => u.UsersSubscribingToThisUser)
                .HasForeignKey(u => u.userSubscribedId)
                 .Metadata.DeleteBehavior = DeleteBehavior.Restrict;


            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages);

            builder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany(u => u.ReceivedMessages);
                

        }

        
    }

    public static class DbContextExtensions
    {
        public static void Seed(this ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Category seed_cat = new Category();
            seed_cat.Description = "A category crrated for seeding";
            seed_cat.Name = "Conventions";
            seed_cat.UrlSlug = "con";
            context.Categories.Add(seed_cat);

            Category second_seed_category = new Category();
            second_seed_category.Description = "a second category for fewer posts";
            second_seed_category.Name ="Anime";
            second_seed_category.UrlSlug = "anime";
            context.Categories.Add(second_seed_category);

            Category third_seed_category = new Category();
            third_seed_category.Description = "third category for seed";
            third_seed_category.Name = "Image Galleries";
            third_seed_category.UrlSlug = "image_galleries";
            context.Categories.Add(third_seed_category);

            Category fourth_seed_category = new Category();
            fourth_seed_category.Description = "category 4";
            fourth_seed_category.Name = "Game Reviews";
            fourth_seed_category.UrlSlug = "game_reviews";
            context.Categories.Add(fourth_seed_category);


            Category fifth_seed_category = new Category();
            fifth_seed_category.Description = "category 5";
            fifth_seed_category.Name = "Videos";
            fifth_seed_category.UrlSlug = "videos";
            context.Categories.Add(fifth_seed_category);

            Tag seed_tag = new Tag();
            seed_tag.Name = "seed tag";
            seed_tag.UrlSlug = "slug";
            seed_tag.Description = " tag created for seeding";

            Tag seed_tag_2 = new Tag();
            seed_tag_2.Name = "tag2";
            seed_tag_2.UrlSlug = "slug2";
            seed_tag_2.Description = "the second tag created for seed";

            Tag seed_tag_3 = new Tag();
            seed_tag_3.Name = "tag3";
            seed_tag_3.UrlSlug = "tagslug3";
            seed_tag_3.Description = "the third seed tag";

            IList<Tag> tagg = new List<Tag>();
            tagg.Add(seed_tag);
            tagg.Add(seed_tag_2);

            IList<Tag> tags2 = new List<Tag>();
            tags2.Add(seed_tag);
            tags2.Add(seed_tag_3);

            context.Tags.Add(seed_tag);
            context.Tags.Add(seed_tag_2);
            context.Tags.Add(seed_tag_3);

            IBlogRepository seedRepo = new BlogRepository(context);

            BlogUser user1 = seedRepo.RetrieveUser("admin@gmail.com");
            BlogUser user2 = seedRepo.RetrieveUser("admin2@gmail.com");
           /* BlogUser user1 = new BlogUser();
            user1.user_name = "admin@gmail.com";
            user1.CategoryBlogUsers = new List<CategoryBlogUser>();
            user1.AuthoredPosts = new List<Post>();

            BlogUser user2 = new BlogUser();
            user2.user_name = "admin2@gmail.com";
            user2.CategoryBlogUsers = new List<CategoryBlogUser>();
            user2.AuthoredPosts = new List<Post>();*/

           
           // context.Update(user1);
            //context.Update(user2);

            string generic_short_description = "<p> Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. <p>";
            string generic_description = generic_short_description + "<p> this is a second paragraph which will only display with the full post";


            for (int i = 1; i <42; i++)
            {
                Post post = new Post();
                post.Title = "seed post" + i.ToString();
                post.UrlSlug = "seed_post_" + i.ToString();
                post.PostedOn = DateTime.Now.AddDays(-i);
                post.PostTags = new List<PostTag>();

                foreach (Tag tag in tagg)
                {
                    PostTag pt = new PostTag();
                    pt.Tag = tag;
                    pt.TagId = tag.TagId;
                    pt.Post = post;
                    pt.PostId = post.PostId;
                    
                    post.PostTags.Add(pt);
                    context.Add(pt);
                }

                if (i < 12)
                {
                   
                    post.Category = seed_cat;
                    user1.AuthoredPosts.Add(post);
                }
                else if (i < 20)
                {
                    post.Category = second_seed_category;
                    user2.AuthoredPosts.Add(post);
                }
                else if(i < 32)
                {
                    post.Category = third_seed_category;
                    user2.AuthoredPosts.Add(post);
                }
                else if(i < 38) {

                    post.Category = fourth_seed_category;
                    user2.AuthoredPosts.Add(post);
                }
                else
                {
                    post.Category = fifth_seed_category;
                    user2.AuthoredPosts.Add(post);
                }
                

                post.ShortDescription = generic_short_description;
                post.Description = generic_description;
                post.Published = true;

                context.Posts.Add(post);


            }

            Message message1 = new Message();
            user1.SentMessages = new List<Message>();
            user2.ReceivedMessages = new List<Message>();

            message1.Sender = user1;
            message1.Recipient = user2;

            user1.SentMessages.Add(message1);
            user2.ReceivedMessages.Add(message1);

            context.Messages.Add(message1);

     
           

           
            BlogUser me = seedRepo.RetrieveUser("andrewjones232@gmail.com");
            BlogUser sender = seedRepo.RetrieveUser("messagetest@gmail.com");

            seedRepo.AuthorizeUser(me.user_name, sender.user_name);
            Message testMessage = new Message();
            testMessage.Contents = "this is a test message which will hopefully display properly.";
            testMessage.Subject = "does it work?";
            testMessage.Sender = sender;
            testMessage.Recipient = me;
            IMessageRepository messageRepo = new MessageRepository(context);

            messageRepo.SendMessage(testMessage);

            MessageCreationViewModel testMessageViewModel = new MessageCreationViewModel();
            testMessageViewModel.AuthorName = "messagetest@gmail.com";
            testMessageViewModel.RecipientName = "andrewjones232@gmail.com";
            testMessageViewModel.Subject = "test2 subject - vm";
            testMessageViewModel.Contents = "this was sent with the view model";
            testMessageViewModel.sendMessage(messageRepo);

            context.SaveChanges();
        }
    }

    
}
