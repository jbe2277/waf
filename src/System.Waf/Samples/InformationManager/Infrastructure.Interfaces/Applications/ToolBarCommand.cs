using System;
using System.Windows.Input;

namespace Waf.InformationManager.Infrastructure.Interfaces.Applications
{
    /// <summary>
    /// Defines a tool bar command
    /// </summary>
    public class ToolBarCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolBarCommand"/> class.
        /// </summary>
        /// <param name="command">The command which is invoked when the user clicks on the tool bar button.</param>
        /// <param name="text">The text of the tool bar button.</param>
        /// <param name="toolTip">The tooltip of the tool bar button.</param>
        /// <exception cref="ArgumentNullException">command must not be null.</exception>
        /// <exception cref="ArgumentException">text must not be null or empty.</exception>
        public ToolBarCommand(ICommand command, string text, string toolTip = null)
        {
            if (command == null) { throw new ArgumentNullException(nameof(command)); }
            if (string.IsNullOrEmpty(text)) { throw new ArgumentException("text must not be null or empty.", nameof(text)); }
            Command = command;
            Text = text;
            ToolTip = toolTip ?? "";
        }

        /// <summary>
        /// Gets the command which is invoked when the user clicks on the tool bar button.
        /// </summary>
        public ICommand Command { get; }

        /// <summary>
        /// Gets the text of the tool bar button.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the tool tip of the tool bar button.
        /// </summary>
        public string ToolTip { get; }
    }
}
