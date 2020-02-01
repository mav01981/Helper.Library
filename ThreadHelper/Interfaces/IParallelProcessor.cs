using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helper.Thread
{
    public interface IParallelProcessor
    {
        Task DoWorkByQueue<T>(int maxThreads, IEnumerable<Action> jobs);
        Task DoWork<T>(int maxThreads, IEnumerable<Task> jobs);
        void DoWork<T>(int maxThreads, IEnumerable<Action> jobs);
    }
}
