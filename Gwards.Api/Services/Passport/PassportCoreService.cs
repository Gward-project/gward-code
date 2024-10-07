using Gward.Common.Enums;
using Gward.Common.Exceptions;
using Gwards.Api.Models.Configurations;
using Gwards.Api.Models.Dto.Nft;
using Gwards.Api.Services.Ton;
using Gwards.DAL;
using Gwards.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using TonSdk.Core;

namespace Gwards.Api.Services.Passport;

public class PassportCoreService
{
    private readonly NftPassportConfiguration _configuration;
    
    private readonly NftMinterService _nftMinterService;
    private readonly NftMetadataGenerator _nftMetadataGenerator;

    private readonly GwardsContext _dbContext;

    public PassportCoreService(
        NftPassportConfiguration configuration,
        NftMinterService nftMinterService,
        NftMetadataGenerator nftMetadataGenerator,
        GwardsContext dbContext
    )
    {
        _configuration = configuration;
        _nftMinterService = nftMinterService;
        _nftMetadataGenerator = nftMetadataGenerator;
        _dbContext = dbContext;
    }

    public Task<int> GetIdByMinter(string minterAddress)
    {
        return _dbContext
            .NftPassports
            .Where(x => x.MinterAddress == minterAddress)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }
    
    public async Task<NftPassportInfoDto> GetInfoByUserId(int userId)
    {
        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);
        
        if (user == null)
        {
            throw new GwardException("User was not found");
        }
        
        var passport = await _dbContext
            .NftPassports
            .FirstOrDefaultAsync(x => x.MinterAddress == user.TonAddress);

        return new NftPassportInfoDto
        {
            Address = passport?.Address,
            MetadataPath = passport?.MetadataPath,
            ImagePath = passport?.ImagePath,
            IsEnoughScoreForMint = IsEnoughScoreForMint(user),
            IsPassportUpgradable = IsPassportUpgradable(user, passport)
        };
    }
    
    public async Task<NftPassportInfoDto> GetInfoByMinterAddress(string minterAddress)
    {
        var passport = await _dbContext
            .NftPassports
            .FirstOrDefaultAsync(x => x.MinterAddress == minterAddress);

        return new NftPassportInfoDto
        {
            Address = passport?.Address,
            MetadataPath = passport?.MetadataPath,
            ImagePath = passport?.ImagePath,
        };
    }

    public bool IsEnoughScoreForMint(UserEntity user)
    {
        return user.Score >= _configuration.MinScoreRequired;
    }

    public bool IsPassportUpgradable(UserEntity user, NftPassportEntity passport)
    {
        if (passport == null)
        {
            return false;
        }
        
        return user.Score > passport.Score;
    }

    public async Task Mint(UserEntity user)
    {
        await _nftMinterService.Mint(
            user.TonAddress,
            _configuration.CollectionAddress,
            OnMintSuccess(user.Id),
            OnMintFailure()
        );
    }

    public async Task UpdateMetadata(int userId, int passportId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        var passport = await _dbContext.NftPassports.FirstOrDefaultAsync(x => x.Id == passportId);

        if (user.TonAddress != passport.MinterAddress)
        {
            throw new InvalidOperationException("User's address and passport minter address must be the same");
        }
        
        passport.Score = user.Score;
        
        await _nftMetadataGenerator.GeneratePassportMetadata(passport.Index, _configuration.BaseMetadataPath, user);
        await _dbContext.SaveChangesAsync();
    }
    
    private Func<IServiceProvider, uint, string, CancellationToken, Task> OnMintSuccess(int userId)
    {
        return async (serviceProvider, nftIndex, mintedNftAddress, cancellationToken) =>
        {
            using var scope = serviceProvider.CreateScope();
            var passportsConfiguration = scope.ServiceProvider.GetService<NftPassportConfiguration>();
            var metadataGenerator = scope.ServiceProvider.GetService<NftMetadataGenerator>();
            var dbContext = scope.ServiceProvider.GetService<GwardsContext>();
            
            var payment = await dbContext.Payments
                .Include(x => x.User)
                .Where(x =>
                    x.UserId == userId &&
                    x.PostPaymentMethod == PostPaymentMethod.MintNftPassport &&
                    x.Transaction != null)
                .OrderByDescending(x => x.CreatedAt)
                .FirstAsync(cancellationToken);

            var metadataPath = Path.Join(passportsConfiguration.BaseMetadataPath, $"{nftIndex}.json");
            var imagePath = Path.Join(passportsConfiguration.BaseMetadataPath, $"{nftIndex}.png");
            var passport = new NftPassportEntity
            {
                Index = nftIndex,
                Address = new Address(mintedNftAddress).ToString(),
                MinterAddress = new Address(payment.User.TonAddress).ToString(),
                MetadataPath = metadataPath,
                ImagePath = imagePath,
                Score = payment.User.Score,
                Payment = payment,
                CreatedAt = DateTime.UtcNow
            };

            await metadataGenerator.GeneratePassportMetadata(nftIndex, passportsConfiguration.BaseMetadataPath, payment.User);
            await dbContext.NftPassports.AddAsync(passport, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        };
    }
    
    private Func<IServiceProvider, CancellationToken, Task> OnMintFailure()
    {
        return (_, _) => Task.CompletedTask;
    }
}