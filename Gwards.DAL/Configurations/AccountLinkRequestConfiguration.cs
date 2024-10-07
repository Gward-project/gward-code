using Gwards.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gwards.DAL.Configurations;

internal class AccountLinkRequestConfiguration : BaseEntityConfiguration<AccountLinkRequestEntity>
{
    public override void Configure(EntityTypeBuilder<AccountLinkRequestEntity> builder)
    {
        base.Configure(builder);
    }
}
