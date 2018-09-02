namespace System.Waf.Applications.Services
{
    /// <summary>
    /// Event arguments for a settings error.
    /// </summary>
    public class SettingsErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the SettingsErrorEventArgs.
        /// </summary>
        /// <param name="error">The exception that occurred.</param>
        public SettingsErrorEventArgs(Exception error)
        {
            Error = error;
        }

        /// <summary>
        /// The exception that occurred.
        /// </summary>
        public Exception Error { get; }
    }
}
