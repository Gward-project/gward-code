using Gwards.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gwards.DAL.Configurations;

internal class ExternalAccountConfiguration : BaseEntityConfiguration<ExternalAccountEntity>
{
    public override void Configure(EntityTypeBuilder<ExternalAccountEntity> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.User).WithMany(x => x.ExternalAccounts).HasForeignKey(x => x.UserId);
    }
}
