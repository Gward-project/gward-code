using Gwards.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gwards.DAL.Configurations;

internal class PaymentConfiguration : BaseEntityConfiguration<PaymentEntity>
{
    public override void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        base.Configure(builder);
        
        builder
            .HasOne(x => x.Transaction)
            .WithOne()
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Payments)
            .OnDelete(DeleteBehavior.Cascade);
    }
}