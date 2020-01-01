using System.Threading;
using System.Threading.Tasks;

namespace System.Waf.Foundation
{
    /// <summary>
    /// Defines the throttling mode.
    /// </summary>
    public enum ThrottledActionMode
    {
        /// <summary>
        /// Invokes the method at maximum once for the delay time.
        /// </summary>
        InvokeMaxEveryDelayTime,

        /// <summary>
        /// Invokes the method only if no invoke request was called for duration of the delay time.
        /// </summary>
        InvokeOnlyIfIdleForDelayTime
    }


    /// <summary>
    /// This class supports throttling of multiple method calls to improve the responsiveness of an application.
    /// It delays a method call and skips all additional calls of this method during the delay.
    /// The call of the action is synchronized. It uses the current synchronization context that was active during creating this class.
    /// </summary>
    /// <remarks>This class is thread-safe.</remarks>
    public class ThrottledAction
    {
        private readonly TaskScheduler taskScheduler;
        private readonly object cancellationTokenSourceLock = new object();
        private readonly Action action;
        private readonly ThrottledActionMode mode;
        private readonly TimeSpan delayTime;
        private CancellationTokenSource? cancellationTokenSource;


        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottledAction"/> class.
        /// </summary>
        /// <param name="action">The action that should be throttled.</param>
        /// <exception cref="ArgumentNullException">The argument action must not be null.</exception>
        public ThrottledAction(Action action)
            : this(action, ThrottledActionMode.InvokeMaxEveryDelayTime, TimeSpan.FromMilliseconds(10))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottledAction"/> class.
        /// </summary>
        /// <param name="action">The action that should be throttled.</param>
        /// <param name="mode">Defines the throttling mode.</param>
        /// <param name="delayTime">The delay time.</param>
        /// <exception cref="ArgumentNullException">The argument action must not be null.</exception>
        public ThrottledAction(Action action, ThrottledActionMode mode, TimeSpan delayTime)
        {
            taskScheduler = SynchronizationContext.Current != null ? TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Default;
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.mode = mode;
            this.delayTime = delayTime;
        }


        /// <summary>
        /// Indicates that an execution of the action delegate is requested.
        /// </summary>
        public bool IsRunning => cancellationTokenSource != null;


        /// <summary>
        /// Requests the execution of the action delegate.
        /// </summary>
        public void InvokeAccumulated()
        {
            CancellationToken? token = null;
            lock (cancellationTokenSourceLock)
            {
                if (mode == ThrottledActionMode.InvokeOnlyIfIdleForDelayTime || cancellationTokenSource == null)
                {
                    cancellationTokenSource?.Cancel();
                    cancellationTokenSource = new CancellationTokenSource();
                    token = cancellationTokenSource.Token;
                }
            }

            if (token != null)
            {
                Task.Delay(delayTime, token.Value).ContinueWith(t =>
                {
                    lock (cancellationTokenSourceLock)
                    {
                        cancellationTokenSource = null;
                    }
                    TaskHelper.Run(action, taskScheduler);
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
            }
        }

        /// <summary>
        /// Cancel the execution of the action delegate that was requested.
        /// </summary>
        public void Cancel()
        {
            lock (cancellationTokenSourceLock)
            {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource = null;
            }
        }
    }
}
