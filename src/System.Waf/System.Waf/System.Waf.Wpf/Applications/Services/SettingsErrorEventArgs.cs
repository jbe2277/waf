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
        /// <param name="action">Describes the action that caused this event.</param>
        /// <param name="fileName">Gets the settings file name.</param>
        public SettingsErrorEventArgs(Exception error, SettingsServiceAction action, string fileName)
        {
            Error = error;
            Action = action;
            FileName = fileName;
        }

        /// <summary>
        /// The exception that occurred.
        /// </summary>
        public Exception Error { get; }
        
        /// <summary>
        /// Describes the action that caused this event.
        /// </summary>
        public SettingsServiceAction Action { get; }

        /// <summary>
        /// Gets the settings file name.
        /// </summary>
        public string FileName { get; }
    }
}
