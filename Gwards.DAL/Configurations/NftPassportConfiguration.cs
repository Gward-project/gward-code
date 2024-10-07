using Gwards.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gwards.DAL.Configurations;

internal class NftPassportConfiguration : BaseEntityConfiguration<NftPassportEntity>
{
    public override void Configure(EntityTypeBuilder<NftPassportEntity> builder)
    {
        base.Configure(builder);

        builder
            .HasIndex(x => x.Address)
            .IsUnique();

        builder
            .HasIndex(x => x.MinterAddress)
            .IsUnique();

        builder
            .HasOne(x => x.Payment)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);
    }
}