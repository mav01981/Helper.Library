using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Helper.Thread
{
    /// <summary>
    /// Peform Parallel Threaded work via Tasks/Void Methods. 
    /// </summary>
    public class ParallelProcessor : IParallelProcessor
    {

        /// <summary>
        /// Process multiple Actions concurrently using concurrent queue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="maxThreads"></param>
        /// <param name="jobs"></param>
        /// <returns></returns>
        public async Task DoWorkByQueue<T>(int maxThreads, IEnumerable<Action> jobs)
        {
            var queue = new ConcurrentQueue<Action>(jobs);
            var tasks = new List<Task>();

            for (int taskNumber = 0; taskNumber < maxThreads; taskNumber++)
            {
                tasks.Add(Task.Run(() =>
                {
                    while (queue.TryDequeue(out Action job))
                    {
                        job.Invoke();
                    }
                }));
            }

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (AggregateException)
            {
                throw;
            }
        }

        /// <summary>
        /// Process multiple Tasks concurrently.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="maxThreads"></param>
        /// <param name="jobs"></param>
        /// <returns></returns>
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

            try
            {
                await Task.WhenAll(allTasks);
            }
            catch (AggregateException)
            {
                throw;
            }
        }

        /// <summary>
        /// Process multiple Actions concurrently.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="maxThreads"></param>
        /// <param name="jobs"></param>
        public void DoWork<T>(int maxThreads, IEnumerable<Action> jobs)
        {
            var exceptions = new ConcurrentQueue<Exception>();

            var options = new ParallelOptions() { MaxDegreeOfParallelism = maxThreads };

            Parallel.ForEach(jobs, options, job =>
            {
                try
                {
                    job.Invoke();
                }
                catch (Exception e)
                {
                    exceptions.Enqueue(e);
                }
            });

            if (exceptions.Count > 0) throw new AggregateException(exceptions);
        }
    }
}
