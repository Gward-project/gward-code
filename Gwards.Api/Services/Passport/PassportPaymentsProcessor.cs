using Gward.Common.Enums;
using Gward.Common.Exceptions;
using Gwards.Api.Models.Dto.Payments;
using Gwards.Api.Services.Ton;
using Gwards.DAL;
using Microsoft.EntityFrameworkCore;

namespace Gwards.Api.Services.Passport;

public class PassportPaymentsProcessor
{
    private readonly PaymentService _paymentService;
    private readonly PassportCoreService _coreService;
    
    private readonly GwardsContext _dbContext;

    public PassportPaymentsProcessor(
        PaymentService paymentService,
        PassportCoreService coreService,
        GwardsContext dbContext
    )
    {
        _paymentService = paymentService;
        _coreService = coreService;
        _dbContext = dbContext;
    }

    public async Task<PaymentInfoDto> StartMintPayment(int userId)
    {
        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);
        
        if (user == null)
        {
            throw new GwardException("User was not found");
        }

        if (string.IsNullOrEmpty(user.TonAddress))
        {
            throw new GwardException("User should must provide ton address to mint NFT passport");
        }

        var existingPassport = await _coreService.GetInfoByMinterAddress(user.TonAddress);
        if (existingPassport.Address != null)
        {
            throw new GwardException("User already has NFT passport");
        }
        
        if (!_coreService.IsEnoughScoreForMint(user))
        {
            throw new GwardException("Not enouth score for mint");
        }
        
        var createPaymentDto = new CreatePaymentDto
        {
            User = user,
            PostPaymentMethod = PostPaymentMethod.MintNftPassport,
        };
        
        return await _paymentService.CreatePayment(createPaymentDto);
    }

    public async Task FinalizeMintPayment(int userId)
    {
        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);
        
        if (user == null)
        {
            throw new GwardException("User was not found");
        }

        var isPaymentComplete = await _paymentService.IsPaymentComplete(userId, PostPaymentMethod.MintNftPassport);
        if (!isPaymentComplete)
        {
            throw new GwardException("Payment was not completed");
        }
        
        await _coreService.Mint(user);
    }
    
    public async Task<PaymentInfoDto> StartMetadataUpdatePayment(int userId)
    {
        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            throw new GwardException("User was not found");
        }

        var passportInfo = await _coreService.GetInfoByUserId(user.Id);
        if (passportInfo.Address == null)
        {
            throw new GwardException("User doesn't have NFT passport");
        }

        if (!passportInfo.IsPassportUpgradable)
        {
            throw new GwardException("Passport is not upgradable");
        }
        
        var createPaymentDto = new CreatePaymentDto
        {
            User = user,
            PostPaymentMethod = PostPaymentMethod.UpdatePassportMetadata,
        };
        
        return await _paymentService.CreatePayment(createPaymentDto);
    }
    
    public async Task FinalizeMetadataUpdatePayment(int userId)
    {
        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);
        
        if (user == null)
        {
            throw new GwardException("User was not found");
        }

        var isPaymentComplete = await _paymentService.IsPaymentComplete(userId, PostPaymentMethod.UpdatePassportMetadata);
        if (!isPaymentComplete)
        {
            throw new GwardException("Payment was not completed");
        }

        var passportId = await _coreService.GetIdByMinter(user.TonAddress);
        
        await _coreService.UpdateMetadata(userId, passportId);
    }
}