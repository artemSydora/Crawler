using Microsoft.EntityFrameworkCore;
using System.Data;
using Crawler.Entities;

namespace Crawler.Repository
{
    public class CrawlerDbContext : DbContext, IEfRepositoryDbContext
    {
        public CrawlerDbContext(DbContextOptions<CrawlerDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Test> Tests { get; set; }

        public DbSet<MeasuredLink> LinkPerformanceResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MeasuredLinkConfiguration());
            modelBuilder.ApplyConfiguration(new TestConfiguration());
        }
    }
}
