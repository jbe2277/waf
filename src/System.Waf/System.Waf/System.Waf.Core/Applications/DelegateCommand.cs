using System.Windows.Input;

namespace System.Waf.Applications
{
    /// <summary>
    /// Provides an <see cref="ICommand"/> implementation which relays the <see cref="Execute"/> and <see cref="CanExecute"/> 
    /// method to the specified delegates.
    /// </summary>
    public class DelegateCommand : IDelegateCommand
    {
        private readonly Action<object?> execute;
        private readonly Func<object?, bool>? canExecute;

        /// <summary>Initializes a new instance of the <see cref="DelegateCommand"/> class.</summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action execute) : this(execute, null) { }

        /// <summary>Initializes a new instance of the <see cref="DelegateCommand"/> class.</summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action<object?> execute) : this(execute, null) { }

        /// <summary>Initializes a new instance of the <see cref="DelegateCommand"/> class.</summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action execute, Func<bool>? canExecute)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));
            this.execute = p => execute();
            this.canExecute = canExecute == null ? null : p => canExecute!();
        }

        /// <summary>Initializes a new instance of the <see cref="DelegateCommand"/> class.</summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        /// <exception cref="ArgumentNullException">The execute argument must not be null.</exception>
        public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        /// <summary>Returns a disabled command.</summary>
        public static DelegateCommand DisabledCommand { get; } = new(() => { }, () => false);

        /// <inheritdoc />
        public event EventHandler? CanExecuteChanged;

        /// <inheritdoc />
        public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;

        /// <inheritdoc />
        public void Execute(object? parameter)
        {
            if (!CanExecute(parameter)) return;
            execute(parameter);
        }

        /// <inheritdoc />
        public void RaiseCanExecuteChanged() => OnCanExecuteChanged(EventArgs.Empty);

        /// <summary>Raises the <see cref="CanExecuteChanged"/> event.</summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnCanExecuteChanged(EventArgs e) => CanExecuteChanged?.Invoke(this, e);

        /// <summary>Raises the <see cref="ICommand.CanExecuteChanged"/> event of the specified commands.</summary>
        /// <param name="commands">The commands.</param>
        /// <exception cref="ArgumentNullException">commands must not be null.</exception>
        /// <exception cref="ArgumentException">commands must not be an empty list.</exception>
        public static void RaiseCanExecuteChanged(params IDelegateCommand[] commands) 
        {
            if (commands is null) throw new ArgumentNullException(nameof(commands));
            if (commands.Length < 1) throw new ArgumentException("commands must not be an empty list.", nameof(commands));
            foreach (var x in commands) x.RaiseCanExecuteChanged(); 
        }
    }
}
