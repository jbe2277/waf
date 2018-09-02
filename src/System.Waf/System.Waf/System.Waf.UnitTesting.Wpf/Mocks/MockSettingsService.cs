using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using System.Waf.Applications.Services;

namespace System.Waf.UnitTesting.Mocks
{
    /// <summary>
    /// Mock for the ISettingsService interface.
    /// </summary>
    [Export(typeof(ISettingsService)), Export]
    public class MockSettingsService : ISettingsService
    {
        private readonly ConcurrentDictionary<Type, Lazy<object>> services = new ConcurrentDictionary<Type, Lazy<object>>();

        /// <summary>
        /// Gets or sets a delegate that is called when Get is called.
        /// </summary>
        public Func<Type, object> GetStub { get; set; }

        /// <summary>
        /// Gets or sets a delegate that is called when Save is called.
        /// </summary>
        public Action SaveStub { get; set; }

        /// <summary>
        /// Gets or sets the settings file name.
        /// </summary>
        /// <exception cref="InvalidOperationException">Setter must not be called anymore if one of the methods was used.</exception>
        public string FileName { get; set; }

        /// <summary>
        /// An error occurred.
        /// </summary>
        public event EventHandler<SettingsErrorEventArgs> ErrorOccurred;

        /// <summary>
        /// Gets the specified user settings object. 
        /// If the requested settings could not be loaded the user settings object with default values will be returned.
        /// Exceptions: This method does not throw any exceptions. Instead register for the ErrorOccurred event.
        /// </summary>
        /// <typeparam name="T">The type of the user settings object.</typeparam>
        /// <returns>The user settings object.</returns>
        public T Get<T>() where T : class, new()
        {
            return (T)(GetStub?.Invoke(typeof(T)) ?? services.GetOrAdd(typeof(T), new Lazy<object>(() => new T())).Value);
        }

        /// <summary>
        /// Saves all user setting objects. Save is also called when this service gets disposed to ensure that the latest changes are persisted.
        /// Exceptions: This method does not throw any exceptions. Instead register for the ErrorOccurred event.
        /// </summary>
        public void Save()
        {
            SaveStub?.Invoke();
        }

        /// <summary>
        /// Raise the ErrorOccurred event.
        /// </summary>
        /// <param name="e">The error event args.</param>
        public void RaiseErrorOccurred(SettingsErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }
    }
}
