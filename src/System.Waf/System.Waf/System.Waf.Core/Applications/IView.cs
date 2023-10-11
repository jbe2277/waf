namespace System.Waf.Applications
{
#if NET6_0_OR_GREATER
    /// <summary>Represents a view. Implement either DataContext or BindingContext but not both.</summary>
#else
    /// <summary>Represents a view.</summary>
#endif
    public interface IView
    {
        /// <summary>Gets or sets the data context of the view.</summary>
#if NET6_0_OR_GREATER
        object? DataContext { get => BindingContext; set => BindingContext = value; }

        /// <summary>Gets or sets the binding context of the view.</summary>
        object? BindingContext { get => null; set { } }
#else
        object? DataContext { get; set; }
#endif
    }
}
