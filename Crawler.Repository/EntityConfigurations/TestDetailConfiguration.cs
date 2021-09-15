using Crawler.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crawler.Repository.EntityConfigurations
{
    public class TestDetailConfiguration : IEntityTypeConfiguration<TestDetail>
    {
        public void Configure(EntityTypeBuilder<TestDetail> builder)
        {
            builder.Property(ml => ml.Url)
                   .HasMaxLength(2048)
                   .IsRequired();
        }
    }
}
