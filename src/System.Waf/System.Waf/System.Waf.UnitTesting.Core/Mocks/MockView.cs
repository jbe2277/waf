using System.Waf.Applications;

namespace System.Waf.UnitTesting.Mocks
{
    /// <summary>Mock class that implements the IView interface.</summary>
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
}
