using System;

namespace ThreadHelper.Events
{
    public class JobCompletedEventArgs : EventArgs, IJobCompletedEventArgs
    {
        public int JobCount { get; set; }
        public string JobName { get; set; }
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }
    }

    public interface IJobCompletedEventArgs
    {
        int JobCount { get; set; }
        DateTime StartTime { get; set; }
        double Duration { get; set; }
    }
}
