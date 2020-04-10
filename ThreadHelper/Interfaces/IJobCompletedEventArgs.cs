using System;

namespace Helper.Thread
{
    public interface IJobCompletedEventArgs
    {
        double Duration { get; set; }
        int JobCount { get; set; }
        DateTime StartTime { get; set; }
    }
}