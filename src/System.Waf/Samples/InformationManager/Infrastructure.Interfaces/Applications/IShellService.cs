namespace Waf.InformationManager.Infrastructure.Interfaces.Applications
{
    /// <summary>Exposes the functionality of the shell.</summary>
    public interface IShellService : INotifyPropertyChanged
    {
        /// <summary>Gets the shell view. Use this object as owner when you need to show a modal dialog.</summary>
        object ShellView { get; }

        /// <summary>Gets or sets the content view which is shown by the shell.</summary>
        object? ContentView { get; set; }

        /// <summary>Adds the specified tool bar commands.</summary>
        /// <param name="commands">The tool bar commands.</param>
        void AddToolBarCommands(IReadOnlyList<ToolBarCommand> commands);

        /// <summary>Clears the tool bar commands.</summary>
        void ClearToolBarCommands();
    }
}
