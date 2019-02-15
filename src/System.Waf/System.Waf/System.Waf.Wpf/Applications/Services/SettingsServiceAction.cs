namespace System.Waf.Applications.Services
{
    /// <summary>Describes the action that caused a settings ErrorOccurred event.</summary>
    public enum SettingsServiceAction
    {
        /// <summary>Open and read a settings file.</summary>
        Open,
        /// <summary>Saves a settings file.</summary>
        Save
    }
}
