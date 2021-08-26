using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crawler.Entities;

namespace Crawler.Repository
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.Property(t => t.HomePageUrl)
                   .HasMaxLength(100)
                   .IsRequired(true);
        }
    }
}
