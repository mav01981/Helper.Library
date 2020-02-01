using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helper.Thread
{
    public interface ISequentialProcessor
    {
        Task DoWork(int maxThreads, IEnumerable<Action> jobs);
        Task DoWork(int maxThreads, IEnumerable<Task> jobs);
    }
}