using System;

namespace Helper.Thread
{
    public class JobCompletedEventArgs : EventArgs, IJobCompletedEventArgs
    {
        public int JobCount { get; set; }
        public string JobName { get; set; }
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }
    }
}
