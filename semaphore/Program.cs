


namespace semaphore;



public class Semaphore(int total)
{
    private readonly SemaphoreSlim _sema = new(total);

    public static async Task Main(string[] args)
    {
        var s = new Semaphore(3);
        var tasks = Enumerable.Range(1, 20).Select(s.ProcessAsync);

        //await Task.WhenAll(tasks);
        Task.WaitAll(tasks);
    }

    private async Task ProcessAsync( int id)
    {
        await _sema.WaitAsync();

        try
        {
            Console.WriteLine($"Calling {id}");
            await Task.Delay(3000);
        }
        finally
        {
            _sema.Release();
        }
    }
}