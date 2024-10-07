using Gward.Common.Constants;
using Gward.Common.Enums;
using Gwards.DAL.Entities;
using Gwards.DAL.Entities.Quests;
using Microsoft.EntityFrameworkCore;

namespace Gwards.DAL;

public class GwardsContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<DailyRewardEntity> DailyRewards { get; set; }
    public DbSet<ExternalAccountEntity> ExternalAccounts { get; set; }
    public DbSet<AccountLinkRequestEntity> AccountLinkRequest { get; set; }
    public DbSet<QuestBaseEntity> Quests { get; set; }
    public DbSet<NftRewardEntity> NftRewards { get; set; }
    public DbSet<UserQuestEntity> UserQuests { get; set; }
    public DbSet<GameQuestEntity> GameQuests { get; set; }
    public DbSet<NftPassportEntity> NftPassports { get; set; }
    public DbSet<PaymentEntity> Payments { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }

    public GwardsContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GwardsContext).Assembly);
        SeedDefaultQuests(modelBuilder);
    }

    private void SeedDefaultQuests(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WalletQuestEntity>().HasData(
            new WalletQuestEntity
            {
                Id = CommonQuestsIds.ConnectTelegramWallet,
                Title = "Connect ton wallet",
                Description = "Connect you're ton wallet to Gward's telegram mini app",
                CreatedAt = DateTime.UtcNow,
                ImagePath = "/static/Images/toncoin.png",
                IsDefault = true,
                RewardType = RewardType.Points,
                Reward = 50
            }
        );
        
        modelBuilder.Entity<GameQuestEntity>().HasData(
            new GameQuestEntity
            {
                Id = CommonQuestsIds.PlayBananaGame,
                Title = "Play banana game",
                Description = "Play banana game from Steam",
                CreatedAt = DateTime.UtcNow,
                ImagePath = "/static/Images/banana.png",
                IsDefault = true,
                RewardType = RewardType.Nft,
                NftCollectionAddress = "EQDdFMFso8gYIaDxWvslY1pXnIoWeGIzLl8_hkWRuPk-xviT",
                NftImagePath = "/static/NftImages/BananaNFT.png",
                NftMetadataBasePath = "/static/NftCollections/BananaCollection",
                ApplicationId = 2923300,
                Platform = "steam"
            }
        );
        
        modelBuilder.Entity<MintQuestEntity>().HasData(
            new MintQuestEntity
            {
                Id = CommonQuestsIds.MintBananaNft,
                NftQuestId = CommonQuestsIds.PlayBananaGame,
                Title = "Mint banana achievement",
                Description = "Mint NFT achievement from 'Play banana game' quest",
                CreatedAt = DateTime.UtcNow,
                ImagePath = "/static/Images/banana.png",
                IsDefault = true,
                RewardType = RewardType.Points,
                Reward = 200,
            }
        );
        
        modelBuilder.Entity<SubscribeQuestEntity>().HasData(
            new SubscribeQuestEntity
            {
                Id = CommonQuestsIds.SubscribeOnTelegram,
                Title = "Join our Telegram channel",
                Description = "Join our Gward telegram channel at https://t.me/gwardxyz",
                CreatedAt = DateTime.UtcNow,
                ImagePath = "/static/Images/telegram.png",
                IsDefault = true,
                RewardType = RewardType.Points,
                Reward = 50
            }
        );
    }
}
