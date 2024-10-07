using System.Text;
using Gwards.Api.Constants;
using Gwards.Api.Models;
using Gwards.Api.Models.Dto.Transactions.Processing;
using TonSdk.Core;
using TonSdk.Core.Boc;

namespace Gwards.Api.Services.Ton;

public class NftMinterService
{
    private readonly TransactionsQueue _transactionsQueue;

    public NftMinterService(TransactionsQueue transactionsQueue)
    {
        _transactionsQueue = transactionsQueue;
    }

    public async Task Mint(
        string minterAddress,
        string collectionAddress,
        Func<IServiceProvider, uint, string, CancellationToken, Task> onSuccess,
        Func<IServiceProvider, CancellationToken, Task> onFailure
    )
    {
        var collection = new Address(collectionAddress);
        var minter = new Address(minterAddress);

        var transactionQueueItem = new TransactionQueueItemDto
        {
            TransactionSendingTask = ProcessTransaction(collection, minter),
            PostProcessingOnSuccessTask = PostProcessTransaction(onSuccess, onFailure),
            PostProcessingOnFailureTask = onFailure
        };

        await _transactionsQueue.QueueTransaction(transactionQueueItem);
    }

    private Func<IServiceProvider, CancellationToken, Task<ProcessedTransactionInfoDto>> ProcessTransaction(Address collection, Address minter)
    {
        return async (serviceProvider, cancellationToken) =>
        {
            using var scope = serviceProvider.CreateScope();
            var tonService = scope.ServiceProvider.GetRequiredService<TonService>();
            var minterService = scope.ServiceProvider.GetRequiredService<NftMinterService>();
            
            var collectionOnChainData = await tonService.GetNftCollectionInfo(collection.ToString());
            var nextNftIndex = uint.Parse(collectionOnChainData.NextItemIndex);
            var mintBodyCell = minterService.GetMintBodyCell(nextNftIndex, minter);
            var mintValue = new Coins(TonConstants.MintPrice);

            var transaction = await tonService.SendTransaction(collection, mintValue, mintBodyCell, cancellationToken);

            return new ProcessedTransactionInfoDto
            {
                TransactionDto = transaction,
                Metadata = nextNftIndex
            };
        };
    }

    private Func<IServiceProvider, ProcessedTransactionInfoDto, CancellationToken, Task> PostProcessTransaction(
        Func<IServiceProvider, uint, string, CancellationToken, Task> onSuccess,
        Func<IServiceProvider, CancellationToken, Task> onFailure
    )
    {
        return async (serviceProvider, transactionInfo, cancellationToken) =>
        {
            if (transactionInfo.TransactionDto == null)
            {
                await onFailure(serviceProvider, cancellationToken);
                return;
            }
            
            if (transactionInfo.Metadata is not uint nextNftIndex)
            {
                await onFailure(serviceProvider, cancellationToken);
                return;
            }
                
            using var scope = serviceProvider.CreateScope();
            var tonService = scope.ServiceProvider.GetRequiredService<TonService>();
            
            var collectionAddress = transactionInfo.TransactionDto.OutMsgs[0].Destination;
            var collection = new Address(collectionAddress);
            var nftAddress = await tonService.TonClient.Nft.GetItemAddress(collection, nextNftIndex);

            await onSuccess(
                serviceProvider,
                nextNftIndex,
                nftAddress.ToString(),
                cancellationToken
            );
        };
    }

    private Cell GetMintBodyCell(uint nftIndex, Address minterAddress)
    {
        var commonContentUrl = $"{nftIndex}.json";
        var urlContent = new CellBuilder()
            .StoreBytes(Encoding.UTF8.GetBytes(commonContentUrl))
            .Build();
        var nftContent = new CellBuilder()
            .StoreAddress(minterAddress)
            .StoreRef(urlContent)
            .Build();

        var transferredMintValue = new Coins(TonConstants.MintedTransferredValue);

        return new CellBuilder()
            .StoreUInt(TonConstants.MintOpCode, 32) // OP code
            .StoreUInt(0, 64) // Query id
            .StoreUInt(nftIndex, 64) // Item index
            .StoreCoins(transferredMintValue) // Transferred value to new contract
            .StoreRef(nftContent) // Nft item content
            .Build();
    }
}