using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pocs.OriginalBehavior
{
    class Program
    {
        static void Main()
        {

            try
            {
                JobSystemExecute();
            }
            catch (Exception ex)
            {
                //Uncomment this to see what the most of the Logger will log, in this case the JobSystem. This is the same that
                //Console.WriteLine(ex.ToString());
            }
        }

        static void JobSystemExecute()
        {
            AppJob();
        }

        static void AppJob()
        {
            try
            {
                var taskList = new List<Task>();
                Split(taskList);
                Task.WaitAll(taskList.ToArray());
            }
            catch (AggregateException aggregateException)
            {
                // This will call the logger N times for each inner exception, making easier to find it in the logs.
                foreach (var ex in aggregateException.Flatten().InnerExceptions)
                {
                    // Emulating a call to the logger.
                    Console.WriteLine(ex.ToString());
                }
                throw;
            }
            catch (Exception exception)
            {
                // This should be calling the logger with the exception.
                Console.WriteLine("Exception thrown: {0}.", exception.ToString());
                throw;
            }
        }
        static void Split(List<Task> taskList)
        {
            taskList.Add(Task.Run(() => LoopAndSend()));
        }

        static void LoopAndSend()
        {
            var taskList = new List<Task>
            {
                Send("Error from child 1"),
                Send("Error from child 2")
            };
            Task.WaitAll(taskList.ToArray());
        }

        private static async Task Send(string message)
        {
            // Try catch has been removed with the instruction throw exception; 
            // Simulate the real 'await Send(...)'
            await Task.FromException(new Exception(message));

        }
    }
}
