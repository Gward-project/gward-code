using Gward.Common.Enums;
using Gward.Common.Exceptions;
using Gwards.Api.Models.Configurations;
using Gwards.Api.Models.Dto.Payments;
using Gwards.DAL;
using Gwards.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using TonSdk.Core;

namespace Gwards.Api.Services.Ton;

public class PaymentService
{
    private readonly PaymentsConfiguration _configuration;

    private readonly TonService _tonService;
    private readonly GwardsContext _dbContext;

    public PaymentService(
        PaymentsConfiguration configuration,
        TonService tonService,
        GwardsContext dbContext
    )
    {
        _configuration = configuration;
        _tonService = tonService;
        _dbContext = dbContext;
    }

    public async Task<PaymentInfoDto> CreatePayment(CreatePaymentDto dto)
    {
        var existingPayment = await _dbContext
            .Payments
            .FirstOrDefaultAsync(x =>
                x.UserId == dto.User.Id &&
                x.PostPaymentMethod == dto.PostPaymentMethod &&
                x.Transaction == null
            );

        if (existingPayment != null)
        {
            _dbContext.Payments.Remove(existingPayment);
        }

        var price = GetPrice(dto.PostPaymentMethod);
        var comment = GetComment(dto.User.Id, dto.PostPaymentMethod);

        var payment = new PaymentEntity
        {
            Amount = price,
            Comment = comment,
            PostPaymentMethod = dto.PostPaymentMethod,
            User = dto.User,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Payments.AddAsync(payment);
        await _dbContext.SaveChangesAsync();

        return new PaymentInfoDto
        {
            Address = _configuration.PayoutAddress,
            Amount = price,
            Comment = comment
        };
    }

    public async Task<bool> IsPaymentComplete(int userId, PostPaymentMethod method)
    {
        var payment = await _dbContext
            .Payments
            .Include(x => x.User)
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.PostPaymentMethod == method &&
                x.Transaction == null
            );
        
        if (payment == null)
        {
            throw new GwardException("Payment was not found");
        }
        
        var tonTransaction = await _tonService.GetTransactionByCommentAndValue(_configuration.PayoutAddress, payment.Amount, payment.Comment);
        if (tonTransaction == null)
        {
            return false;
        }

        var transaction = new TransactionEntity
        {
            Hash = tonTransaction.Hash,
            Source = tonTransaction.InMsg.Source,
            Destination = tonTransaction.InMsg.Destination,
            Value = tonTransaction.InMsg.Value,
            Comment = tonTransaction.InMsg.MsgData.Decoded.Comment,
            CreatedAt = DateTime.UtcNow
        };

        payment.Transaction = transaction;

        await _dbContext.Transactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    private string GetPrice(PostPaymentMethod method)
    {
        return method switch
        {
            PostPaymentMethod.MintNftPassport => new Coins(_configuration.MintNftPassportPrice).ToNano(),
            PostPaymentMethod.UpdatePassportMetadata => new Coins(_configuration.UpdatePassportMetadata).ToNano(),
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, null)
        };
    }
    
    private string GetComment(int userId, PostPaymentMethod method)
    {
        return $"{userId}_{method}_{Guid.NewGuid():N}";
    }
}