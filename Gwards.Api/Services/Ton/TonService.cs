using Gwards.Api.Models.Configurations;
using Gwards.Api.Models.Dto.Nft;
using Gwards.Api.Models.Dto.Transactions.TonApi;
using Newtonsoft.Json;
using TonSdk.Client;
using TonSdk.Contracts.Wallet;
using TonSdk.Core;
using TonSdk.Core.Block;
using TonSdk.Core.Boc;
using TonSdk.Core.Crypto;

namespace Gwards.Api.Services.Ton;

public class TonService
{
    public TonClient TonClient => _tonClient;
    
    private readonly TonConfiguration _configuration;
    private readonly ILogger<TonService> _logger;
    
    private readonly HttpClient _httpClient;
    private readonly TonClient _tonClient;
    
    private readonly KeyPair _senderKeyPair;
    private readonly WalletV4 _senderWallet;
    
    public TonService(
        TonConfiguration configuration,
        ILogger<TonService> logger
    )
    {
        _configuration = configuration;
        _logger = logger;

        var httpParams = new HttpParameters
        {
            Endpoint = configuration.ApiUrl,
            ApiKey = configuration.ApiKey,
        };

        _httpClient = new HttpClient();
        _tonClient = new TonClient(TonClientType.HTTP_TONCENTERAPIV3, httpParams);
        
        var senderSeed = Mnemonic.GenerateSeed(configuration.MasterWalletMnemonic);
        var senderKeyPair = Mnemonic.GenerateKeyPair(senderSeed);
        var walletOptions = new WalletV4Options
        {
            PublicKey = senderKeyPair.PublicKey,
            Workchain = configuration.WorkChain
        };

        _senderKeyPair = senderKeyPair;
        _senderWallet = new WalletV4(walletOptions);
    }
    
    public async Task<ICollection<TransactionDto>> GetTransactionsByAddress(string address, int limit, int offset)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_configuration.ApiUrl}transactions?account={address}&limit={limit}&offset={offset}&sort=desc"),
            Headers = { { "x-api-key", _configuration.ApiKey } }
        };

        var response = await _httpClient.SendAsync(request);
        var strResult = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException($"Invalid response from Ton Center:\n{strResult}");
        }

        var serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        var parsedResult = JsonConvert.DeserializeObject<RootTransactionsDto>(strResult, serializerSettings);
        if (parsedResult == null)
        {
            throw new ApplicationException($"Response from Ton Center cannot be parsed:\n{strResult}");
        }

        return parsedResult.Transactions;
    }

    public async Task<TransactionDto> GetTransactionByInMsgHash(string inMsgHash, string senderAddress, CancellationToken cancellationToken)
    {
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_configuration.ApiUrl}transactions?account={senderAddress}&limit=10&offset=0&sort=desc"),
            Headers = { { "x-api-key", _configuration.ApiKey } }
        };
        
        var response = await _httpClient.SendAsync(message, cancellationToken);
        var strResult = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException($"Invalid response from Ton Center:\n{strResult}");
        }

        var serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        var parsedResult = JsonConvert.DeserializeObject<RootTransactionsDto>(strResult, serializerSettings);
        if (parsedResult == null)
        {
            throw new ApplicationException($"Response from Ton Center cannot be parsed:\n{strResult}");
        }

        return parsedResult.Transactions.FirstOrDefault(x => x.InMsg.Hash == inMsgHash);
    }
    
    public async Task<TransactionDto> GetTransactionByCommentAndValue(string receiverAddress, string value, string comment)
    {
        var transactions = await GetTransactionsByAddress(receiverAddress, 100, 0);
        return transactions
            .FirstOrDefault(transaction =>
                transaction.InMsg.Value == value &&
                transaction.InMsg?.MsgData?.Decoded?.Comment == comment
            );
    }
    
    public async Task<NftCollectionInfoDto> GetNftCollectionInfo(string collectionAddress)
    {
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_configuration.ApiUrl}nft/collections?collection_address={collectionAddress}&limit=10&offset=0"),
            Headers = { { "x-api-key", _configuration.ApiKey } }
        };
        
        var response = await _httpClient.SendAsync(message);
        var strResult = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException($"Invalid response from Ton Center:\n{strResult}");
        }

        var serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        var parsedResult = JsonConvert.DeserializeObject<RootNftCollectionInfoDto>(strResult, serializerSettings);
        if (parsedResult == null)
        {
            throw new ApplicationException($"Response from Ton Center cannot be parsed:\n{strResult}");
        }

        if (parsedResult.NftCollections.Length == 0)
        {
            throw new ApplicationException($"Nft collection with address \"{collectionAddress}\" was not found");
        }

        return parsedResult.NftCollections[0];
    }

    public async Task<TransactionDto> SendTransaction(
        Address destAddress,
        Coins value,
        Cell body,
        CancellationToken cancellationToken = default
    )
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }
        
        _logger.LogInformation($"Start sending transaction to: {destAddress.ToString()}");
        
        var messageInfoOptions = new IntMsgInfoOptions
        {
            Dest = destAddress,
            Value = value
        };
        
        var internalMessage = new InternalMessage(new()
        {
            Info = new IntMsgInfo(messageInfoOptions),
            Body = body
        });
        
        var transfer = new WalletTransfer
        {
            Message = internalMessage,
            Mode = 1
        };

        var currentWalletSeqno = await _tonClient.Wallet.GetSeqno(_senderWallet.Address);
        if (currentWalletSeqno is not { } walletSeqno)
        {
            throw new ApplicationException("Sender wallet was not deployed yet.");
        }

        var message = _senderWallet.CreateTransferMessage([transfer], walletSeqno).Sign(_senderKeyPair.PrivateKey);
        var messageHash = message.Cell.Hash.ToString();
        var sendBocResult = await _tonClient.SendBoc(message.Cell);

        if (sendBocResult is not { })
        {
            throw new ApplicationException($"Couldn't sent transaction to address {destAddress}");
        }
        
        _logger.LogInformation($"Sent transaction to: {destAddress.ToString()}\nBoc result hash: {messageHash}");
        
        var transaction = await WaitTransactionForComplete(messageHash, _senderWallet.Address.ToString(), cancellationToken);
        
        _logger.LogInformation($"Transaction to: {destAddress.ToString()} was successfully sent!");

        return transaction;
    }
    
    private async Task<TransactionDto> WaitTransactionForComplete(string inMsgHash, string senderAddress, CancellationToken cancellationToken = default)
    {
        const int retryAttempts = 15;
        const int sleepDelayMilliseconds = 3000;
        
        for (var i = 0; i < retryAttempts; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }
            
            var transaction = await GetTransactionByInMsgHash(inMsgHash, senderAddress, cancellationToken);
            if (transaction != null)
            {
                return transaction;
            }
            
            await Task.Delay(sleepDelayMilliseconds, cancellationToken);
        }

        throw new ApplicationException("Could not wait for transaction processing to complete.");
    }
}