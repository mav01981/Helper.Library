using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ThreadHelper;
using Xunit;

namespace Tests
{
    public class SequentialTests
    {
        private void MethodDoWork()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        public async Task Sequential_1Job_Raises1JobCount()
        {
            //Arrange.
            List<Action> jobs = new List<Action>();

            for (int i = 0; i < 1; i++)
            {
                jobs.Add(new Action(MethodDoWork));
            }

            var processor = new Sequential();

            processor.JobCompleted += (sender, e) =>
               {
                   Assert.Equal(1, e.JobCount);
               };

            //Act.
            await processor.DoWork(1, jobs);
            //Assert.
        }

        [Fact]
        public async Task Sequential_2Jobs_Raises2JobCompletedEvents()
        {
            //Arrange.
            List<Action> jobs = new List<Action>();

            for (int i = 0; i < 2; i++)
            {
                jobs.Add(new Action(MethodDoWork));
            }

            var processor = new Sequential();

            int numberOfEvents = 0;

            processor.JobCompleted += (sender, e) =>
            {
                numberOfEvents++;
            };

            //Act.
            await processor.DoWork(2, jobs);
            //Assert.
            Assert.True(numberOfEvents == 2);
        }
    }
}
