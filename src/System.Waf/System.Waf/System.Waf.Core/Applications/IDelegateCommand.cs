using System.Windows.Input;

namespace System.Waf.Applications
{
    /// <summary>Extend the ICommand with support to notify that CanExecute has changed.</summary>
    public interface IDelegateCommand : ICommand
    {
        /// <summary>Raises the <see cref="ICommand.CanExecuteChanged"/> event.</summary>
        void RaiseCanExecuteChanged();
    }
}
