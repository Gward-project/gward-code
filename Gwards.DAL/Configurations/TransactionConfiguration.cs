using Gwards.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gwards.DAL.Configurations;

internal class TransactionConfiguration : BaseEntityConfiguration<TransactionEntity>
{
    public override void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.Hash).IsUnique();
    }
}