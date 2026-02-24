using System.Collections.Concurrent;

namespace queue;




public class Queue
{
    public static async Task Main(string[] args)
    {
        var q = new ConcurrentQueue<int>();
        var token = new CancellationTokenSource();

       var cancelation =  Task.Run(() =>
        {
            token.CancelAfter(6000);
        });

        Task producer = Task.Run(() =>
        {
            for (int i = 0; i < 100; i++)
            {
                q.Enqueue(i);
            }
        });

        Task  consumer = Task.Run(async () =>
        {
            while (!token.Token.IsCancellationRequested)
            {
                if (q.TryDequeue(out int item))
                {
                    await ProcessAsync(item);
                }else
                {
                    await Task.Delay(10);
                }
            }
        });


        await Task.WhenAll(producer, consumer, cancelation);
    }

    private static async Task ProcessAsync(int i)
    {
        Console.WriteLine($"Message gotten {i}");
        await Task.Delay(300);
    }


}