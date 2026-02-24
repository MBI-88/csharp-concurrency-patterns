namespace concurrency;



public class ConcurrencyTasks
{
    private int  counter = 0;
    readonly private object _lock = new object();
    public static async Task Main(string[] args)
    {   
        var task = new ConcurrencyTasks();
        
        Parallel.For(1, 10, i =>
        {
            lock (task._lock)
            {

                task.counter++;
                Console.WriteLine($"Worker {i} done {task.counter}");
            }
            Thread.Sleep(3);
        });

    }
}