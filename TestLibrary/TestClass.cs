using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Articles.Models;
using Articles.Data;

namespace TestLibrary
{
    [TestFixture]
    public class TestClass
    {
        public TestClass()
        {
        }

        [TestCase]
        public void firstTest()
        {
            Assert.That(2, Is.EqualTo(1 + 1));
        }

        [TestCase]
        public void PostsByCategoryTest()
        {
            TestServiceProvider provider = new TestServiceProvider();
            IBlogRepository repository = provider.GetBlogRepository();
            ApplicationDbContext context = provider.GetDbContext();

            Category testCategory = new Category();
            testCategory.Name = "Test Category";
            testCategory.UrlSlug = "testcat";
            IList<Post> testList = new List<Post>();
           
            Post testPost1 = new Post();
            testPost1.Category = testCategory;
            testPost1.Description = "test description";
            testPost1.Published = true;
            testPost1.Title = "test title";

            Post testPost2 = new Post();
            testPost2.Category = testCategory;
            testPost2.Description = "test description2";
            testPost2.Published = true;
            testPost2.Title = "test title2";

            Post testPost3 = new Post();
            testPost3.Category = testCategory;
            testPost3.Description = "test description3";
            testPost3.Published = true;
            testPost3.Title = "test title3";

            testList.Add(testPost1);
            testList.Add(testPost2);
            testList.Add(testPost3);

            testCategory.Posts = testList;

            context.Categories.Add(testCategory);
            context.Posts.Add(testPost1);
            context.Posts.Add(testPost2);
            context.Posts.Add(testPost3);
            context.SaveChanges();

            IList<Post> repositoryList = repository.PostsForCategory(testCategory.UrlSlug, 0, 10);

            Assert.That(repositoryList, Is.EquivalentTo(testList));

            context.Posts.Remove(testPost1);
            context.Posts.Remove(testPost2);
            context.Posts.Remove(testPost3);
            context.Categories.Remove(testCategory);
            context.SaveChanges();
        }
    }
}
