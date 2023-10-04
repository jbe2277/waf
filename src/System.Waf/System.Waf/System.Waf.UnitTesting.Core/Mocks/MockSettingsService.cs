using System.Collections.Concurrent;
using System.Waf.Applications.Services;

namespace System.Waf.UnitTesting.Mocks
{
    /// <summary>Mock for the ISettingsService interface.</summary>
    public class MockSettingsService : ISettingsService
    {
        private readonly ConcurrentDictionary<Type, Lazy<object>> services = new();

        /// <summary>Gets or sets a delegate that is called when Get is called.</summary>
        public Func<Type, object>? GetStub { get; set; }

        /// <summary>Gets or sets a delegate that is called when Save is called.</summary>
        public Action? SaveStub { get; set; }

        /// <inheritdoc />
        public string FileName { get; set; } = default!;

        /// <inheritdoc />
        public event EventHandler<SettingsErrorEventArgs>? ErrorOccurred;

        /// <inheritdoc />
        public T Get<T>() where T : class, new() => (T)(GetStub?.Invoke(typeof(T)) ?? services.GetOrAdd(typeof(T), new Lazy<object>(() => new T())).Value);

        /// <inheritdoc />
        public void Save() => SaveStub?.Invoke();

        /// <summary>Raise the ErrorOccurred event.</summary>
        /// <param name="e">The error event args.</param>
        public void RaiseErrorOccurred(SettingsErrorEventArgs e) => ErrorOccurred?.Invoke(this, e);
    }
}
