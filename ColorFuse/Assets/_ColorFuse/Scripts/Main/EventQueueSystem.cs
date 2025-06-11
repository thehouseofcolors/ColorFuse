using System.Collections.Generic;
using System.Threading.Tasks;

public class EventQueue
{
    private Queue<IGameEvent> queue = new Queue<IGameEvent>();
    private bool isProcessing = false;

    public void Enqueue(IGameEvent gameEvent)
    {
        queue.Enqueue(gameEvent);
        if (!isProcessing)
            _ = ProcessQueue();
    }

    private async Task ProcessQueue()
    {
        isProcessing = true;

        while (queue.Count > 0)
        {
            var currentEvent = queue.Dequeue();
            await EventBus.PublishAsync(currentEvent);
        }

        isProcessing = false;
    }
}
