using Gward.Common.Enums;
using Gward.Common.Exceptions;
using Gwards.Api.Models.Dto.Nft;
using Gwards.Api.Models.Dto.Quests;
using Gwards.Api.Services.Ton;
using Gwards.DAL;
using Gwards.DAL.Entities;
using Gwards.DAL.Entities.Quests;
using Gwards.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using TonSdk.Core;

namespace Gwards.Api.Services;

public class QuestService
{
    private readonly GwardsContext _gwardsContext;
    private readonly NftMinterService _nftMinter;

    public QuestService(
        GwardsContext gwardsContext,
        NftMinterService nftMinter
    )
    {
        _gwardsContext = gwardsContext;
        _nftMinter = nftMinter;
    }

    public async Task<List<QuestInfoDto>> GetUserActiveQuests(int userId)
    {
        var quests = await _gwardsContext
            .UserQuests
            .Include(x => x.Quest)
            .Where(x => x.UserId == userId && x.Status != QuestStatus.Claimed).ToListAsync();
        
        return quests.Select(x => new QuestInfoDto
        {
            Id = x.Id,
            Title = x.Quest.Title,
            Description = x.Quest.Description,
            RewardType = x.Quest.RewardType,
            ImagePath = x.Quest.ImagePath,
            RewardNftImagePath = x.Quest.NftImagePath,
            Type = x.Quest.Type,
            UserId = x.UserId,
            Status = x.Status,
            Reward = x.Quest.Reward,
        }).ToList();
    }

    public async Task<ICollection<NftRewardDto>> GetUserQuestRewards(int userId)
    {
        var user = await _gwardsContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            throw new GwardException("User was not found");
        }

        if (user.TonAddress == null)
        {
            return [];
        }

        return await _gwardsContext.NftRewards
            .Where(x =>
                x.MinterAddress == user.TonAddress
            )
            .Select(x => new NftRewardDto
            {
                Address = x.Address,
                ImagePath = x.Quest.NftImagePath,
                CollectionMetadataPath = x.Quest.NftMetadataBasePath + "/metadata.json",
                ItemMetadataPath = x.Quest.NftMetadataBasePath + $"/{x.Index}.json",
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }
    
    public async Task Claim(int questId, int userId)
    {
        var userQuest = await _gwardsContext
            .UserQuests
            .Include(x => x.Quest)
            .FirstOrDefaultAsync(x => x.Id == questId && x.UserId == userId);

        if (userQuest == null)
        {
            throw new GwardException("Quest not found");
        }

        if (userQuest.Status != QuestStatus.ReadyForClaim)
        {
            return;
        }

        var user = await _gwardsContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            throw new GwardException("User not found");
        }
        
        if (userQuest.Quest.RewardType == RewardType.Points)
        {
            userQuest.Status = QuestStatus.Claimed;
            userQuest.ClaimedAt = DateTime.UtcNow;
            user.GwardPointBalance += userQuest.Quest?.Reward ?? 0;
            
            await _gwardsContext.SaveChangesAsync();
        }

        if (userQuest.Quest.RewardType == RewardType.Nft)
        {
            if (string.IsNullOrEmpty(user.TonAddress))
            {
                throw new GwardException("User doesn't have ton address");
            }
            
            userQuest.Status = QuestStatus.WaitForNftMint;
            
            await _gwardsContext.SaveChangesAsync();
            await _nftMinter.Mint(
                user.TonAddress,
                userQuest.Quest.NftCollectionAddress,
                OnMintSuccess(userQuest.Id),
                OnMintFailure(userQuest.Id)
            );
        }
    }

    private Func<IServiceProvider, uint, string, CancellationToken, Task> OnMintSuccess(int userQuestId)
    {
        return async (serviceProvider, nftIndex, mintedNftAddress, cancellationToken) =>
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<GwardsContext>();
            var metadataGenerator = scope.ServiceProvider.GetService<NftMetadataGenerator>();
            
            var userQuest = await dbContext
                .UserQuests
                .Include(x => x.Quest)
                .Include(x => x.User)
                .FirstAsync(x => x.Id == userQuestId, cancellationToken);

            await metadataGenerator.GenerateQuestMetadata(nftIndex, userQuest.Quest);
            
            userQuest.Status = QuestStatus.Claimed;
            userQuest.ClaimedAt = DateTime.UtcNow;

            var reward = new NftRewardEntity
            {
                Index = nftIndex,
                Address = new Address(mintedNftAddress).ToString(),
                MinterAddress = new Address(userQuest.User.TonAddress).ToString(),
                Quest = userQuest.Quest,
                CreatedAt = DateTime.UtcNow
            };

            var mintQuest = await dbContext
                .UserQuests
                .FirstOrDefaultAsync(x =>
                    x.UserId == userQuest.User.Id &&
                    x.Quest.Type == QuestType.Mint &&
                    x.Status == QuestStatus.New &&
                    ((MintQuestEntity)x.Quest).NftQuestId == userQuest.Quest.Id,
                    cancellationToken
                );

            if (mintQuest != null)
            {
                mintQuest.Status = QuestStatus.ReadyForClaim;
            }
            
            await dbContext.NftRewards.AddAsync(reward, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        };
    }
    
    private Func<IServiceProvider, CancellationToken, Task> OnMintFailure(int userQuestId)
    {
        return async (serviceProvider, cancellationToken) =>
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<GwardsContext>();
            var userQuest = await dbContext.UserQuests.FirstAsync(x => x.Id == userQuestId, cancellationToken);
            
            userQuest.Status = QuestStatus.ReadyForClaim;

            await dbContext.SaveChangesAsync(cancellationToken);
        };
    }
}
