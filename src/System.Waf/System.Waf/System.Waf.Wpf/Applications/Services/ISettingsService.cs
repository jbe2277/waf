namespace System.Waf.Applications.Services
{
    /// <summary>
    /// Service that is responsible to load and save user settings.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Gets or sets the settings file name.
        /// </summary>
        /// <exception cref="InvalidOperationException">Setter must not be called anymore if one of the methods was used.</exception>
        string FileName { get; set; }

        /// <summary>
        /// An error occurred.
        /// </summary>
        event EventHandler<SettingsErrorEventArgs> ErrorOccurred;

        /// <summary>
        /// Gets the specified user settings object. 
        /// If the requested settings could not be loaded the user settings object with default values will be returned.
        /// Exceptions: This method does not throw any exceptions. Instead register for the ErrorOccurred event.
        /// </summary>
        /// <typeparam name="T">The type of the user settings object.</typeparam>
        /// <returns>The user settings object.</returns>
        T Get<T>() where T : class, new();

        /// <summary>
        /// Saves all user setting objects. Save is also called when this service gets disposed to ensure that the latest changes are persisted.
        /// Exceptions: This method does not throw any exceptions. Instead register for the ErrorOccurred event.
        /// </summary>
        void Save();
    }
}
