using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Waf.UnitTesting
{
    /// <summary>
    /// Provides a synchronization context for unit tests that simulates the behavior of the WPF or Windows Forms synchronization context.
    /// </summary>
    public sealed class UnitTestSynchronizationContext : SynchronizationContext, IDisposable
    {
        private readonly SynchronizationContext previousContext;
        private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object>> messageQueue;
        private volatile bool isDisposed;


        private UnitTestSynchronizationContext()
        {
            previousContext = SynchronizationContext.Current;
            messageQueue = new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();
        }


        /// <summary>
        /// Gets the unit test synchronization context for the current thread.
        /// </summary>
        public new static UnitTestSynchronizationContext Current => SynchronizationContext.Current as UnitTestSynchronizationContext;


        /// <summary>
        /// Creates a new instance of the unit test synchronization context and sets this instance as current synchronization context for this thread.
        /// </summary>
        /// <returns>The unit test synchronization context.</returns>
        public static UnitTestSynchronizationContext Create()
        {
            var context = new UnitTestSynchronizationContext();
            SetSynchronizationContext(context);
            return context;
        }

        /// <summary>
        /// Creates a copy of this <see cref="UnitTestSynchronizationContext"/>.
        /// </summary>
        /// <returns>The copy of this synchronization context.</returns>
        public override SynchronizationContext CreateCopy()
        {
            return new UnitTestSynchronizationContext();
        }

        /// <summary>
        /// Dispose this synchronization context and sets the previous context back as current synchronization context.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Invokes the callback in the synchronization context synchronously.
        /// </summary>
        /// <param name="d">The delegate to call.</param>
        /// <param name="state">The object passed to the delegate.</param>
        /// <exception cref="ArgumentNullException">d must not be null.</exception>
        public override void Send(SendOrPostCallback d, object state)
        {
            if (d == null) { throw new ArgumentNullException(nameof(d)); }
            if (isDisposed) { return; }
            d(state);
        }

        /// <summary>
        /// Invokes the callback in the synchronization context asynchronously. 
        /// </summary>
        /// <param name="d">The delegate to call.</param>
        /// <param name="state">The object passed to the delegate.</param>
        /// <exception cref="ArgumentNullException">d must not be null.</exception>
        public override void Post(SendOrPostCallback d, object state)
        {
            if (d == null) { throw new ArgumentNullException(nameof(d)); }
            if (isDisposed) { return; }
            messageQueue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
        }

        /// <summary>
        /// Process the message queue until the specified task is completed.
        /// </summary>
        /// <param name="task">The task to wait for completion.</param>
        /// <exception cref="ArgumentNullException">task must not be null.</exception>
        public void Wait(Task task)
        {
            if (task == null) { throw new ArgumentNullException(nameof(task)); }
            var cancellation = new CancellationTokenSource();
            task.ContinueWith(t => cancellation.Cancel());
            ProcessMessageQueue(cancellation.Token);
            task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Process the message queue for the specified time.
        /// </summary>
        /// <param name="time">Defines how long the message queue is processed.</param>
        public void Wait(TimeSpan time)
        {
            ProcessMessageQueue(new CancellationTokenSource(time).Token);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposed) { return; }
            isDisposed = true;

            if (isDisposing)
            {
                FinishMessageQueue();
                messageQueue.Dispose();
                SetSynchronizationContext(previousContext);
            }
        }

        private void ProcessMessageQueue(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var message = messageQueue.Take(token);
                    message.Key(message.Value);
                }
            }
            catch (OperationCanceledException)
            {
                // Abort processing the message queue.
            }
        }

        private void FinishMessageQueue()
        {
            messageQueue.CompleteAdding();
            foreach (var message in messageQueue)
            {
                message.Key(message.Value);
            }
        }
    }
}
