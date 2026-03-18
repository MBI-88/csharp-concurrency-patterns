using System.Threading.Channels;

namespace channel;



public class ChannelModel(int capacity)
{
    private readonly Channel<string> channel = Channel.CreateBounded<string>(capacity);
    public static async Task Main(string[] args)
    {
        var ch = new ChannelModel(3);
        var token = new CancellationTokenSource();
        
        var cancel = Task.Run( ()=>
        {
           token.CancelAfter(3000);
        });

        var pr = Task.Run(async () =>
        {
            for (int i = 1; i < 10; i++)
            {
               await ch.Producer(i, token.Token);
               await Task.Delay(200);
            }
        });

        var co = Task.Run( async () =>
        {
            await ch.Consumer(token.Token);
        });
        
        Task.WaitAll(pr, co, cancel);
    }

    public async Task Producer(int message, CancellationToken t)
    {   
        await channel.Writer.WriteAsync($"Write -> {message}", t);
    }

    public async Task Consumer(CancellationToken t)
    {   
       await foreach (var result in  channel.Reader.ReadAllAsync(t))
        {
            Console.WriteLine($"Read -> {result}");
        };
    }
}
