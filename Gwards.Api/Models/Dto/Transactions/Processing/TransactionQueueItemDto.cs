namespace Gwards.Api.Models.Dto.Transactions.Processing;

public class TransactionQueueItemDto
{
    public Func<IServiceProvider, CancellationToken, Task<ProcessedTransactionInfoDto>> TransactionSendingTask;
    public Func<IServiceProvider, ProcessedTransactionInfoDto, CancellationToken, Task> PostProcessingOnSuccessTask;
    public Func<IServiceProvider, CancellationToken, Task> PostProcessingOnFailureTask;
}
