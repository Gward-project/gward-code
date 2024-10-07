using Gwards.Api.Models;

namespace Gwards.Api.HostedServices;

public class TransactionsProcessingHostedService : BackgroundService
{
    private readonly TransactionsQueue _transactionsQueue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TransactionsProcessingHostedService> _logger;

    public TransactionsProcessingHostedService(
        TransactionsQueue transactionsQueue,
        IServiceProvider serviceProvider,
        ILogger<TransactionsProcessingHostedService> logger
    )
    {
        _transactionsQueue = transactionsQueue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Transaction Processing Hosted Service is running.");

        await BackgroundProcessing(cancellationToken);
    }

    private async Task BackgroundProcessing(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var transactionQueueItem = await _transactionsQueue.DequeueAsync(cancellationToken);

            try
            {
                var transactionInfo = await transactionQueueItem.TransactionSendingTask(_serviceProvider, cancellationToken);
                await transactionQueueItem.PostProcessingOnSuccessTask(_serviceProvider, transactionInfo, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to process transaction, reason: {ex.Message}");
                await transactionQueueItem.PostProcessingOnFailureTask(_serviceProvider, cancellationToken);
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Transaction Processing Hosted Service is stopping.");

        await base.StopAsync(cancellationToken);
    }
}