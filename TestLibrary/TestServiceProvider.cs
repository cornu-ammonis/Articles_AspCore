using Articles.Data;
using Articles.Models;
using Articles.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestLibrary
{
    public class TestServiceProvider
    {
        ApplicationDbContext _context;
        IBlogRepository _repository;
        public TestServiceProvider()
        {
            IServiceCollection collection = new ServiceCollection();
            // collection.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-Articles-db5a80cf-4e8b-49bb-b04a-c7ee3cf07a0b;Trusted_Connection=True;MultipleActiveResultSets=true"
            //   ));
            collection.AddDbContext<ApplicationDbContext>();
            collection.AddScoped<IBlogRepository, BlogRepository>();
            IServiceProvider provider = collection.BuildServiceProvider();
            _context = provider.GetService<ApplicationDbContext>();
            _repository = provider.GetService<IBlogRepository>();

        }

        public IBlogRepository GetBlogRepository()
        {
            return _repository;
        }

        public ApplicationDbContext GetDbContext()
        {
            return _context;
        }

    }
}
