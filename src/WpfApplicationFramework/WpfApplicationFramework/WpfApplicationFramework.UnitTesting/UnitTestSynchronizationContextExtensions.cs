using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Waf.UnitTesting
{
    /// <summary>
    /// Provides extension methods for working with the <see cref="UnitTestSynchronizationContext"/>.
    /// </summary>
    public static class UnitTestSynchronizationContextExtensions
    {
        /// <summary>
        /// Process the message queue until the task is completed.
        /// </summary>
        /// <param name="task">The task to wait for completion.</param>
        /// <param name="context">The current unit test synchronization context.</param>
        /// <exception cref="ArgumentNullException">task must not be null.</exception>
        /// <exception cref="ArgumentNullException">context must not be null.</exception>
        public static void Wait(this Task task, UnitTestSynchronizationContext context)
        {
            if (context == null) { throw new ArgumentNullException("context"); }
            context.Wait(task);
        }

        /// <summary>
        /// Process the message queue until the task is completed and returns the task's result.
        /// </summary>
        /// <param name="task">The task to wait for completion.</param>
        /// <param name="context">The current unit test synchronization context.</param>
        /// <exception cref="ArgumentNullException">task must not be null.</exception>
        /// <exception cref="ArgumentNullException">context must not be null.</exception>
        public static T GetResult<T>(this Task<T> task, UnitTestSynchronizationContext context)
        {
            if (context == null) { throw new ArgumentNullException("context"); }
            context.Wait(task);
            return task.Result;
        }

        /// <summary>
        /// Process the message queue until the predicate returns true. 
        /// This method uses a polling approach and therefore the predicate is called often.
        /// </summary>
        /// <param name="context">The current unit test synchronization context.</param>
        /// <param name="predicate">The predicate returns true when the waiting should end.</param>
        /// <param name="timeout">Defines a timeout for the waiting.</param>
        /// <exception cref="ArgumentNullException">context must not be null.</exception>
        /// <exception cref="ArgumentNullException">predicate must not be null.</exception>
        /// <exception cref="TimeoutException">A timeout occurred.</exception>
        public static void WaitFor(this UnitTestSynchronizationContext context, Func<bool> predicate, TimeSpan timeout)
        {
            if (context == null) { throw new ArgumentNullException("context"); }
            if (predicate == null) { throw new ArgumentNullException("predicate"); }
            var sw = Stopwatch.StartNew();
            while (!predicate())
            {
                if (sw.Elapsed > timeout)
                {
                    throw new TimeoutException();
                }
                context.Wait(TimeSpan.FromMilliseconds(5));
            }
        }
    }
}
