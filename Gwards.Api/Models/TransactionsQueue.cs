using System.Threading.Channels;
using Gwards.Api.Models.Dto.Transactions.Processing;

namespace Gwards.Api.Models;

public class TransactionsQueue
{
    private readonly Channel<TransactionQueueItemDto> _queue;

    public TransactionsQueue(int capacity)
    {
        var options = new BoundedChannelOptions(capacity) { FullMode = BoundedChannelFullMode.Wait };
        _queue = Channel.CreateBounded<TransactionQueueItemDto>(options);
    }

    public async ValueTask QueueTransaction(TransactionQueueItemDto itemDto)
    {
        ArgumentNullException.ThrowIfNull(itemDto);
        await _queue.Writer.WriteAsync(itemDto);
    }

    public async Task<TransactionQueueItemDto> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}