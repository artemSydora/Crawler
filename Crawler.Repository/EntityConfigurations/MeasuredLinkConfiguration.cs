using Crawler.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crawler.Repository
{
    public class MeasuredLinkConfiguration : IEntityTypeConfiguration<MeasuredLink>
    {
        public void Configure(EntityTypeBuilder<MeasuredLink> builder)
        {
            builder.Property(ml => ml.Url)
                   .HasMaxLength(2048)
                   .IsRequired();
        }
    }
}
