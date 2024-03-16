using System.Waf.Applications;

namespace System.Waf.UnitTesting.Mocks
{
    /// <summary>Mock view that implements the IView interface.</summary>
    public class MockView : IView
    {
        /// <inheritdoc/>
#if NET6_0_OR_GREATER
        public object? DataContext { get => BindingContext; set => BindingContext = value; }

        /// <inheritdoc/>
        public object? BindingContext { get; set; }
#else
        public object? DataContext { get; set; }
#endif
    }

    /// <summary>Mock view that implements the IView interface.</summary>
    /// <typeparam name="TViewModel">Type of the associated view model.</typeparam>
    public class MockView<TViewModel> : MockView where TViewModel : class
    {
        /// <summary>The view model.</summary>
        public TViewModel ViewModel => (TViewModel)(DataContext ?? throw new InvalidOperationException("ViewModel is not set."));
    }
}
