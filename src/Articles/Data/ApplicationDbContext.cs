using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Articles.Models;
using Articles.Models.Core;
using Microsoft.AspNetCore.Identity;


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
            seed_cat.Name = "category one";
            seed_cat.UrlSlug = "slug_one";
            context.Categories.Add(seed_cat);

            Category second_seed_category = new Category();
            second_seed_category.Description = "a second category for fewer posts";
            second_seed_category.Name = "seed two";
            second_seed_category.UrlSlug = "seed_two";
            context.Categories.Add(second_seed_category);

            Category third_seed_category = new Category();
            third_seed_category.Description = "third category for seed";
            third_seed_category.Name = "seed category 3";
            third_seed_category.UrlSlug = "seed_category_three";
            context.Categories.Add(third_seed_category);




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



            string generic_short_description = "<p> There are a handful of conventions that seem to stand out above all of the rest. People who are interested in knowing about t.v. shows and movies that will be released over the next year usually pay attention to New York Comicon. People who are interested in video game news look to the Pax conventions eagerly waiting to hear about announcements pertaining to their favorite franchises. However cosplayers wait all year to show case their newest cosplays at Dragoncon. It doesn’t matter if you are looking for panels to teach you how to create a new prop or believe you have what it takes to win a cosplay contest Dragoncon has exactly what you are looking for. <p>";
            string generic_description = generic_short_description + "<p> this is a second paragraph which will only display with the full post";


            for (int i = 1; i <32; i++)
            {
                Post post = new Post();
                post.Title = "seed post" + i.ToString();
                post.UrlSlug = "seed_post_" + i.ToString();
                post.PostedOn = DateTime.Now.AddDays(i);
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
                }
                else if (i < 20)
                {
                    post.Category = second_seed_category;
                }
                else
                {
                    post.Category = third_seed_category;
                }
                

                post.ShortDescription = generic_short_description;
                post.Description = generic_description;
                post.Published = true;

                context.Posts.Add(post);


            }


          


        context.SaveChanges();
        }
    }

    
}
