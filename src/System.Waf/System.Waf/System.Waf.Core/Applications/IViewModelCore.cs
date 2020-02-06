namespace System.Waf.Applications
{
    /// <summary>Interface for a ViewModel.</summary>
    public interface IViewModelCore
    {
        /// <summary>Gets the associated view.</summary>
        object View { get; }

        /// <summary>Initialize the view model.</summary>
        void Initialize();
    }
}
