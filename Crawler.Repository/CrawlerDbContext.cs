using Crawler.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Crawler.Repository
{
    public class CrawlerDbContext : DbContext, IEfRepositoryDbContext
    {
        public CrawlerDbContext(DbContextOptions<CrawlerDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<TestDTO> Tests { get; set; }

        public DbSet<DetailDTO> Details { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CrawlerDbContext).Assembly);
        }
    }
}
