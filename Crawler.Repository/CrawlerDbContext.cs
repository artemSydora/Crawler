using System;
using System.Data;
using System.Reflection;
using Crawler.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Repository
{
    public class CrawlerDbContext : DbContext, IEfRepositoryDbContext
    {
        public CrawlerDbContext(DbContextOptions<CrawlerDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        DbSet<Test> Tests { get; set; }

        DbSet<MeasuredLink> LinkPerformanceResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MeasuredLinkConfiguration());
            modelBuilder.ApplyConfiguration(new TestConfiguration());
        }
    }
}
