using Microsoft.EntityFrameworkCore;
using Crawler.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crawler.Repository
{
    class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.Property(t => t.HomePageUrl)
                   .HasMaxLength(100)
                   .IsRequired(true);
        }
    }
}
