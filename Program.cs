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
                Console.WriteLine(ex.ToString());
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
            catch (Exception exception)
            {

                // Replicate logger error without null-check on InnerException
                // Potential bug, InnerException can be null. Message will return only that there was 1 or more errors, nothing about the internal exceptions.
                Console.WriteLine("Exception thrown: {0}.", exception.InnerException.Message);
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
            try
            {
                // Simulate the real 'await send()'
                await Task.FromException(new Exception(message));
            }
            catch (Exception exception)
            {
                // Same as original: catch and rethrow with 'throw exception;'
                // This is an async/await call so the exception is correctly catpured and propagated (with all the inner exceptions, stacktrace, etc.)
                throw exception;
            }
        }
    }
}
