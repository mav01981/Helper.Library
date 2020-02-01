using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Helper.Thread
{
    public class SequentialProcessor : ISequentialProcessor
    {
        public event EventHandler<JobCompletedEventArgs> JobCompleted;
        protected virtual void OnJobCompleted(JobCompletedEventArgs e)
        {
            JobCompleted?.Invoke(this, e);
        }

        /// <summary>
        ///  Process multiple Actions in a fifo order.
        /// </summary>
        /// <param name="maxThreads"></param>
        /// <param name="jobs"></param>
        /// <returns></returns>
        public async Task DoWork(int maxThreads, IEnumerable<Action> jobs)
        {
            var tasks = new List<Task>();

            int counter = 0;

            foreach (var job in jobs)
            {
                DateTime startTime = DateTime.Now;

                job.Invoke();

                OnJobCompleted(new JobCompletedEventArgs()
                {
                    JobCount = counter + 1,
                    JobName = job.Method.Name,
                    StartTime = startTime,
                    Duration = (DateTime.Now - startTime).TotalMilliseconds
                });

                counter++;
            }
        }

        /// <summary>
        /// Process multiple Tasks in a fifo order.
        /// </summary>
        /// <param name="maxThreads"></param>
        /// <param name="jobs"></param>
        /// <returns></returns>
        public async Task DoWork(int maxThreads, IEnumerable<Task> jobs)
        {
            var throttler = new SemaphoreSlim(initialCount: maxThreads);

            int counter = 1;

            foreach (var job in jobs)
            {
                await throttler.WaitAsync();

                try
                {
                    DateTime startTime = DateTime.Now;

                    await job;

                    OnJobCompleted(new JobCompletedEventArgs()
                    {
                        JobCount = counter,
                        StartTime = startTime,
                        Duration = (DateTime.Now - startTime).TotalMilliseconds
                    });
                }
                finally
                {
                    throttler.Release();
                }

                counter++;
            }
        }
    }
}
