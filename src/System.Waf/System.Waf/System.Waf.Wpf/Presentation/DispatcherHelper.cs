using System.Waf.Applications;

namespace System.Waf.Presentation
{
    /// <summary>
    /// Provides helper methods for working with the dispatcher.
    /// </summary>
    public static class DispatcherHelper
    {
        /// <summary>
        /// Execute the event queue of the dispatcher until all pending messages have been processed.
        /// </summary>
        public static void DoEvents()
        {
            DispatcherHelperCore.DoEvents();
        }
    }
}
