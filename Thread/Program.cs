using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ThreadHelper;

namespace Thread
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var process = new Sequential();

            List<Action> jobs = new List<Action>();

            for (int i = 0; i < 50; i++)
            {
                jobs.Add(new Action(DoPaintWall));
            }

            process.JobCompleted += Process_JobCompleted;

            process.DoWork(2, jobs);

            Console.ReadLine();
        }

        private static void Process_JobCompleted(object sender, ThreadHelper.Events.JobCompletedEventArgs e)
        {
            Console.WriteLine($"Job Count:{e.JobCount}" +
                $"|Job Name:{e.JobName}" +
                $"|Start Time:{e.StartTime}" +
                $"|Duration:{e.Duration}");
        }

        private static void DoPaintWall()
        {
            Console.WriteLine("Wall Painted.");
        }
        private static void DoGardeWork()
        {
            Console.WriteLine("Garden work completed.");
        }
    }
}
