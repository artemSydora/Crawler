using Crawler.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crawler.Repository.EntityConfigurations
{
    public class TestDTOConfiguration : IEntityTypeConfiguration<TestDTO>
    {
        public void Configure(EntityTypeBuilder<TestDTO> builder)
        {
            builder.Property(t => t.StartPageUrl)
                   .HasMaxLength(100)
                   .IsRequired();
        }
    }
}
