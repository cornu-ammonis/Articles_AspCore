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
