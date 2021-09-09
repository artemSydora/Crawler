using Crawler.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crawler.EntityConfigurations
{
    public class TestResultConfiguration : IEntityTypeConfiguration<TestResult>
    {
        public void Configure(EntityTypeBuilder<TestResult> builder)
        {
            builder.Property(t => t.StartPageUrl)
                   .HasMaxLength(100)
                   .IsRequired();
        }
    }
}
