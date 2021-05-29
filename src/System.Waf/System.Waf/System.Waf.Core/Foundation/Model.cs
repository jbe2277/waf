using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System.Waf.Foundation
{
    /// <summary>Defines the base class for a model.</summary>
    [DataContract]
    public abstract class Model : INotifyPropertyChanged
    {
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>Set the property with the specified value. If the value is not equal with the field then the field is set, a PropertyChanged event is raised and it returns true.</summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">Reference to the backing field of the property.</param>
        /// <param name="value">The new value for the property.</param>
        /// <param name="propertyName">The property name. This optional parameter can be skipped because the compiler is able to create it automatically.</param>
        /// <returns>True if the value has changed, false if the old and new value were equal.</returns>
        protected bool SetProperty<T>([NotNullIfNotNull(parameterName: "value"), MaybeNull] ref T field, [AllowNull] T value, [CallerMemberName] string propertyName = null!)
        {
            if (EqualityComparer<T>.Default.Equals(field, value!)) return false;
            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        /// <summary>Raises the <see cref="PropertyChanged"/> event.</summary>
        /// <param name="propertyName">The property name of the property that has changed. This optional parameter can be skipped because the compiler is able to create it automatically.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null!) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        /// <summary>Raises <see cref="PropertyChanged"/> events for all specified properties.</summary>
        /// <param name="propertyNames">The property names of the properties that have changed.</param>
        protected void RaisePropertyChanged(params string[] propertyNames) { foreach (var x in propertyNames ?? throw new ArgumentNullException(nameof(propertyNames))) RaisePropertyChanged(x); }

        /// <summary>Raises the <see cref="PropertyChanged"/> event.</summary>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
    }
}
