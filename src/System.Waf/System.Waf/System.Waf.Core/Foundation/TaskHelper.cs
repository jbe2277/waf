using System.Threading;
using System.Threading.Tasks;

namespace System.Waf.Foundation
{
    /// <summary>
    /// Provides helper methods for working with Tasks.
    /// </summary>
    public static class TaskHelper
    {
        /// <summary>
        /// Queues the specified work to run on the thread pool and returns a Task object that represents that work.
        /// </summary>
        /// <param name="action">The work to execute asynchronously</param>
        /// <param name="scheduler">The TaskScheduler that is used to schedule the created Task.</param>
        /// <returns></returns>
        public static Task Run(Action action, TaskScheduler scheduler)
        {
            return Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.DenyChildAttach, scheduler);
        }

        /// <summary>
        /// Queues the specified work to run on the thread pool and returns a proxy for the task returned by function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The work to execute asynchronously</param>
        /// <param name="scheduler">The TaskScheduler that is used to schedule the created Task.</param>
        /// <returns></returns>
        public static Task<T> Run<T>(Func<T> action, TaskScheduler scheduler)
        {
            return Task<T>.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.DenyChildAttach, scheduler);
        }

        /// <summary>
        /// Use this extension method if a Task should not be (a)waited for. Using this method prevents a compile warning.
        /// The method does nothing.
        /// </summary>
        /// <param name="task">The task.</param>
        public static void NoWait(this Task task)
        {
            // Do nothing
        }
    }
}
