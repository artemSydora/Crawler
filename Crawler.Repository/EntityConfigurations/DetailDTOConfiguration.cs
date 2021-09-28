using Crawler.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crawler.Repository.EntityConfigurations
{
    public class DetailDTOConfiguration : IEntityTypeConfiguration<DetailDTO>
    {
        public void Configure(EntityTypeBuilder<DetailDTO> builder)
        {
            builder.Property(ml => ml.Url)
                   .HasMaxLength(2048)
                   .IsRequired();
        }
    }
}
