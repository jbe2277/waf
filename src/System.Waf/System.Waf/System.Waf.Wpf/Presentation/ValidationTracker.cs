using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace System.Waf.Presentation
{
    // This class listens to the Validation.Error event of the owner (Control). When the Error event is raised then it synchronizes 
    // the errors with its internal errors list and updates the ValidationHelper.
    internal sealed class ValidationTracker
    {
        private readonly List<(object source, ValidationError error)> errors;
        private readonly DependencyObject owner;

        public ValidationTracker(DependencyObject owner)
        {
            this.owner = owner;
            errors = new List<(object, ValidationError)>();
            Validation.AddErrorHandler(owner, ErrorChangedHandler);
        }

        internal void AddErrors(object validationSource, IEnumerable<ValidationError> errors)
        {
            foreach (ValidationError x in errors) AddError(validationSource, x);
            ValidationHelper.InternalSetIsValid(owner, !errors.Any());
        }

        private void AddError(object validationSource, ValidationError error)
        {
            errors.Add((validationSource, error));
            if (validationSource is FrameworkElement element)
            {
                element.Unloaded += ValidationSourceUnloaded;
            }
            else
            {
                ((FrameworkContentElement)validationSource).Unloaded += ValidationSourceUnloaded;
            }
        }

        private void ErrorChangedHandler(object? sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                AddError(e.OriginalSource, e.Error);
            }
            else
            {
                var error = errors.FirstOrDefault(err => err.source == e.OriginalSource && err.error == e.Error);
                if (error.source != null) errors.Remove(error);
            }
            ValidationHelper.InternalSetIsValid(owner, !errors.Any());
        }

        private void ValidationSourceUnloaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                element.Unloaded -= ValidationSourceUnloaded;
            }
            else
            {
                ((FrameworkContentElement)sender).Unloaded -= ValidationSourceUnloaded;
            }

            // An unloaded control might be loaded again. Then we need to restore the validation errors.
            var errorsToRemove = errors.Where(err => err.source == sender).ToArray();
            if (errorsToRemove.Any())
            {
                // It keeps alive because it listens to the Loaded event.
                new ValidationReloadedTracker(this, errorsToRemove[0].source, errorsToRemove.Select(x => x.error));
                foreach (var x in errorsToRemove) errors.Remove(x);
            }
            ValidationHelper.InternalSetIsValid(owner, !errors.Any());
        }
    }
}
