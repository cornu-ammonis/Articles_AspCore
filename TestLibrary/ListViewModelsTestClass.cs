using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Articles.Models;
using Articles.Data;
using Moq;
using Articles.Models.BlogViewModels.ListViewModels;

namespace TestLibrary
{
    [TestFixture]
    public class ListViewModelsTestClass
    {
        public ListViewModelsTestClass()
        {
        }

        [TestCase]
        public void firstTest()
        {
            Assert.That(2, Is.EqualTo(1 + 1));
        }


        [TestCase]
        public void PostsByCategoryViewModelRetrievesPostList()
        {
            var mockRepository = new Mock<IBlogRepository>();
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

            mockRepository.Setup(r => r.PostsForCategory(testCategory.UrlSlug, 0, 10)).Returns(testList);

            ListViewModel catListViewModel = new CategoryListViewModel(mockRepository.Object, testCategory.UrlSlug, 1);

            Assert.That(catListViewModel.Posts, Is.EquivalentTo(testList));
        }

        [TestCase]
        public void PostsByCategoryViewModelRetrievesPostCount()
        {
            var mockRepository = new Mock<IBlogRepository>();
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

            mockRepository.Setup(r => r.TotalPostsForCategory(testCategory.UrlSlug)).Returns(testList.Count);

            ListViewModel catListViewModel = new CategoryListViewModel(mockRepository.Object, testCategory.UrlSlug, 1);

            Assert.That(catListViewModel.TotalPosts, Is.EqualTo(testList.Count));
        }

        [TestCase]
        public void AllPostsViewModelRetrievesPostList()
        {
            var mockRepository = new Mock<IBlogRepository>();

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

            mockRepository.Setup(r => r.Posts(0, 10)).Returns(testList);

            ListViewModel testAllPostsViewModel = new AllPostsListViewModel(mockRepository.Object, 1);

            Assert.That(testAllPostsViewModel.Posts, Is.EquivalentTo(testList));

        }

        [TestCase]
        public void AllPostsViewModelRetrievesPostCount()
        {
            var mockRepository = new Mock<IBlogRepository>();

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

            mockRepository.Setup(r => r.TotalPosts(true)).Returns(testList.Count);

            ListViewModel testAllPostsViewModel = new AllPostsListViewModel(mockRepository.Object, 1);

            Assert.That(testAllPostsViewModel.TotalPosts, Is.EqualTo(testList.Count));

        }

        [TestCase]
        public void SavedPostsListViewModelRetrievesPostList()
        {
            var mockRepository = new Mock<IBlogRepository>();
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

            mockRepository.Setup(r => r.PostsUserSaved("test", 0, 10)).Returns(testList);
            mockRepository.Setup(r => r.TotalPostsUserSaved("test")).Returns(testList.Count);

            ListViewModel savedListViewModel = new SavedListViewModel(mockRepository.Object, 1, "test");

            Assert.That(savedListViewModel.Posts, Is.EquivalentTo(testList));
           
        }

        [TestCase]
        public void SavedPostsListViewModelRetrievesPostCount()
        {
            var mockRepository = new Mock<IBlogRepository>();
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

            mockRepository.Setup(r => r.PostsUserSaved("test", 0, 10)).Returns(testList);
            mockRepository.Setup(r => r.TotalPostsUserSaved("test")).Returns(testList.Count);

            ListViewModel savedListViewModel = new SavedListViewModel(mockRepository.Object, 1, "test");

           
            Assert.That(savedListViewModel.TotalPosts, Is.EqualTo(testList.Count));
        }


        [TestCase]
        public void CustomListViewModelRetrievesPostList()
        {
            var mockRepository = new Mock<IBlogRepository>();
            Post post1 = new Post();
            Post post2 = new Post();
            Post post3 = new Post();
            Post post4 = new Post();

            Category testCat1 = new Category();
            Category testCat2 = new Category();
            post1.Category = testCat1;
            post2.Category = testCat2;
            post3.Category = testCat1;
            post4.Category = testCat2;

            List<Post> postList = new List<Post>();
            postList.Add(post1);
            postList.Add(post2);
            postList.Add(post3);
            postList.Add(post4);
            mockRepository.Setup(r => r.CustomPostsForUser("testUserName", 0, 10)).Returns(postList);
            ListViewModel testViewModel = new CustomListViewModel(mockRepository.Object, 1, "testUserName");

            Assert.That(testViewModel.Posts, Is.EquivalentTo(postList));
        }

        [TestCase]
        public void CustomListViewModelRetrievesPostCount()
        {
            var mockRepository = new Mock<IBlogRepository>();
            Post post1 = new Post();
            Post post2 = new Post();
            Post post3 = new Post();
            Post post4 = new Post();

            Category testCat1 = new Category();
            Category testCat2 = new Category();
            post1.Category = testCat1;
            post2.Category = testCat2;
            post3.Category = testCat1;
            post4.Category = testCat2;

            List<Post> postList = new List<Post>();
            postList.Add(post1);
            postList.Add(post2);
            postList.Add(post3);
            postList.Add(post4);
            mockRepository.Setup(r => r.TotalCustomPostsForUser("testUserName")).Returns(postList.Count);
            ListViewModel testViewModel = new CustomListViewModel(mockRepository.Object, 1, "testUserName");

            Assert.That(testViewModel.TotalPosts, Is.EqualTo(postList.Count));
        }

        [TestCase]
        public void SearchListViewModelRetrievesPostListFromRepository()
        {
            var mockRepository = new Mock<IBlogRepository>();
            Post post1 = new Post();
            Post post2 = new Post();
            Post post3 = new Post();
            Post post4 = new Post();

            Category testCat1 = new Category();
            Category testCat2 = new Category();
            post1.Category = testCat1;
            post2.Category = testCat2;
            post3.Category = testCat1;
            post4.Category = testCat2;

            List<Post> postList = new List<Post>();
            postList.Add(post1);
            postList.Add(post2);
            postList.Add(post3);
            postList.Add(post4);

            mockRepository.Setup(r => r.PostsForSearch("testSearch", 0, 10)).Returns(postList);
            ListViewModel searchListViewModel = new SearchListViewModel(mockRepository.Object, "testSearch", 1);
            Assert.That(searchListViewModel.Posts, Is.EquivalentTo(postList));
        }

        [TestCase]
        public void SearchListViewModelRetrievesPostCountFromRepository()
        {
            var mockRepository = new Mock<IBlogRepository>();
            Post post1 = new Post();
            Post post2 = new Post();
            Post post3 = new Post();
            Post post4 = new Post();

            Category testCat1 = new Category();
            Category testCat2 = new Category();
            post1.Category = testCat1;
            post2.Category = testCat2;
            post3.Category = testCat1;
            post4.Category = testCat2;

            List<Post> postList = new List<Post>();
            postList.Add(post1);
            postList.Add(post2);
            postList.Add(post3);
            postList.Add(post4);

            mockRepository.Setup(r => r.TotalPostsForSearch("testSearch")).Returns(postList.Count);
            ListViewModel searchListViewModel = new SearchListViewModel(mockRepository.Object, "testSearch", 1);
            Assert.That(searchListViewModel.TotalPosts, Is.EqualTo(postList.Count));
        }

        [TestCase]
        public void SubscribedListVIewModelRetrievesPostListFromRepository()
        {
            var mockRepository = new Mock<IBlogRepository>();
            Post post1 = new Post();
            Post post2 = new Post();
            Post post3 = new Post();
            Post post4 = new Post();

            Category testCat1 = new Category();
            Category testCat2 = new Category();
            post1.Category = testCat1;
            post2.Category = testCat2;
            post3.Category = testCat1;
            post4.Category = testCat2;

            List<Post> postList = new List<Post>();
            postList.Add(post1);
            postList.Add(post2);
            postList.Add(post3);
            postList.Add(post4);

            mockRepository.Setup(r => r.SubscribedPostsForUser("testUser", 0, 10)).Returns(postList);
            ListViewModel subscribedListViewModel = new SubscribedListViewModel(mockRepository.Object, 1, "testUser");
            Assert.That(subscribedListViewModel.Posts, Is.EquivalentTo(postList));
        }

        [TestCase]
        public void SubscribedListViewModelRetrievesPostCountFromRepository()
        {
            var mockRepository = new Mock<IBlogRepository>();
            Post post1 = new Post();
            Post post2 = new Post();
            Post post3 = new Post();
            Post post4 = new Post();

            Category testCat1 = new Category();
            Category testCat2 = new Category();
            post1.Category = testCat1;
            post2.Category = testCat2;
            post3.Category = testCat1;
            post4.Category = testCat2;

            List<Post> postList = new List<Post>();
            postList.Add(post1);
            postList.Add(post2);
            postList.Add(post3);
            postList.Add(post4);

            mockRepository.Setup(r => r.TotalSubscribedPostsForUser("testUser")).Returns(postList.Count);
            ListViewModel subscribedListViewModel = new SubscribedListViewModel(mockRepository.Object, 1, "testUser");
            Assert.That(subscribedListViewModel.TotalPosts, Is.EqualTo(postList.Count));
        }

        [TestCase]
        public void AuthorListViewModelRetrievesPostListFromRepository()
        {
            var mockRepository = new Mock<IBlogRepository>();
            Post post1 = new Post();
            Post post2 = new Post();
            Post post3 = new Post();
            Post post4 = new Post();

            Category testCat1 = new Category();
            Category testCat2 = new Category();
            post1.Category = testCat1;
            post2.Category = testCat2;
            post3.Category = testCat1;
            post4.Category = testCat2;

            List<Post> postList = new List<Post>();
            postList.Add(post1);
            postList.Add(post2);
            postList.Add(post3);
            postList.Add(post4);

            mockRepository.Setup(r => r.PostsByAuthor("testAuthorName", 0, 10)).Returns(postList);
            ListViewModel authorListViewModel = new AuthorListViewModel(mockRepository.Object, "testAuthorName", 1, null);
            Assert.That(authorListViewModel.Posts, Is.EquivalentTo(postList));
        }

        [TestCase]
        public void AuthorListViewModelRetrievesPostCountFromRepository()
        {
            var mockRepository = new Mock<IBlogRepository>();
            Post post1 = new Post();
            Post post2 = new Post();
            Post post3 = new Post();
            Post post4 = new Post();

            Category testCat1 = new Category();
            Category testCat2 = new Category();
            post1.Category = testCat1;
            post2.Category = testCat2;
            post3.Category = testCat1;
            post4.Category = testCat2;

            List<Post> postList = new List<Post>();
            postList.Add(post1);
            postList.Add(post2);
            postList.Add(post3);
            postList.Add(post4);

            mockRepository.Setup(r => r.TotalPostsByAuthor("testAuthorName")).Returns(postList.Count);
            ListViewModel authorListViewModel = new AuthorListViewModel(mockRepository.Object, "testAuthorName", 1, null);
            Assert.That(authorListViewModel.TotalPosts, Is.EqualTo(postList.Count));
        }

        /* [TestCase]
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


             IList<Post> repositoryList = repository.PostsForCategory(testCategory.UrlSlug, 0, 10);

             Assert.That(repositoryList, Is.EquivalentTo(testList));

             context.Posts.Remove(testPost1);
             context.Posts.Remove(testPost2);
             context.Posts.Remove(testPost3);
             context.Categories.Remove(testCategory);

         }*/
    }
}
