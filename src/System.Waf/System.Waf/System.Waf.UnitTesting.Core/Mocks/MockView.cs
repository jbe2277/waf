using System.Waf.Applications;

namespace System.Waf.UnitTesting.Mocks
{
    /// <summary>
    /// Mock class that implements the IView interface.
    /// </summary>
    public class MockView : IView
    {
        /// <summary>
        /// Gets or sets the data context of the view.
        /// </summary>
        public object? DataContext { get; set; }
    }
}
