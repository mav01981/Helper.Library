using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadHelper
{
    /// <summary>
    /// Peform Parallel Threaded work via Tasks/Void Methods. 
    /// </summary>
    public class ParallelTasks : IParallel
    {
        public async Task DoWorkByQueue<T>(int maxThreads, IEnumerable<Action> jobs)
        {
            var queue = new ConcurrentQueue<Action>(jobs);
            var tasks = new List<Task>();

            for (int taskNumber = 0; taskNumber < maxThreads; taskNumber++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    while (queue.TryDequeue(out Action job))
                    {
                        job.Invoke();
                    }
                }));
            }
            await Task.WhenAll(tasks);
        }
        public async Task DoWork<T>(int maxThreads, IEnumerable<Task> jobs)
        {
            var allTasks = new List<Task>();
            var throttler = new SemaphoreSlim(initialCount: maxThreads);

            foreach (var job in jobs)
            {
                await throttler.WaitAsync();
                allTasks.Add(
                    Task.Run(async () =>
                    {
                        try
                        {
                            await job;
                        }
                        finally
                        {
                            throttler.Release();
                        }
                    }));
            }
            await Task.WhenAll(allTasks);
        }
        public void DoWork<T>(int maxThreads, IEnumerable<Action> jobs)
        {
            var options = new ParallelOptions() { MaxDegreeOfParallelism = maxThreads };

            Parallel.ForEach(jobs, options, job =>
            {
                job.Invoke();
            });
        }
    }
}
