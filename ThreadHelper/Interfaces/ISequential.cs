using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThreadHelper
{
    public interface ISequential
    {
        Task DoWork(int maxThreads, IEnumerable<Action> jobs);
        Task DoWork(int maxThreads, IEnumerable<Task> jobs);
    }
}