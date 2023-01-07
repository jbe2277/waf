﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;

namespace System.Waf.UnitTesting
{
    /// <summary>This class contains assert methods which can be used in unit tests.</summary>
    public static class AssertHelper
    {
        /// <summary>Asserts that the execution of the provided action throws the specified exception.</summary>
        /// <typeparam name="T">The expected exception type.</typeparam>
        /// <param name="action">The action to execute.</param>
        /// <returns>The expected exception.</returns>
        /// <exception cref="AssertException">Thrown when the expected exception was not thrown by the action.</exception>
        public static T ExpectedException<T>(Action action) where T : Exception
        {
            if (action == null) throw new ArgumentNullException(nameof(action));            
            T? expectedException = null;
            Exception? wrongException = null;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(T))
                {
                    expectedException = (T)ex;
                }
                else
                {
                    wrongException = ex;
                }
            }

            if (wrongException != null)
            {
                throw new AssertException(string.Format(null, "Test method threw exception {0}, but exception {1} was expected. Exception message: {0}: {2}",
                    wrongException.GetType().Name, typeof(T).Name, wrongException.Message));
            }
            else if (expectedException == null)
            {
                throw new AssertException(string.Format(null, "Test method did not throw expected exception {0}", typeof(T).Name));
            }
            return expectedException;
        }

        /// <summary>Asserts that the execution of the provided action raises the CanExecuteChanged event of the command.</summary>
        /// <param name="command">The command.</param>
        /// <param name="raiseCanExecuteChanged">An action that results in a CanExecuteChanged event of the command.</param>
        /// <exception cref="AssertException">This exception is thrown when no or more than one CanExecuteChanged event was 
        /// raised by the command or the sender object of the event was not the command object.</exception>
        public static void CanExecuteChangedEvent(ICommand command, Action raiseCanExecuteChanged)
        {
            CanExecuteChangedEvent(command, raiseCanExecuteChanged, 1, ExpectedChangedCountMode.Exact);
        }

        /// <summary>Asserts that the execution of the provided action raises the CanExecuteChanged event of the command.</summary>
        /// <param name="command">The command.</param>
        /// <param name="raiseCanExecuteChanged">An action that results in a CanExecuteChanged event of the command.</param>
        /// <param name="expectedChangedCount">The expected count of CanExecuteChanged events.</param>
        /// <param name="expectedChangedCountMode">The mode defines how the expected changed count is used as assert condition.</param>
        /// <exception cref="AssertException">This exception is thrown when no or more than one CanExecuteChanged event was 
        /// raised by the command or the sender object of the event was not the command object.</exception>
        public static void CanExecuteChangedEvent(ICommand command, Action raiseCanExecuteChanged, int expectedChangedCount, 
            ExpectedChangedCountMode expectedChangedCountMode)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (raiseCanExecuteChanged == null) throw new ArgumentNullException(nameof(raiseCanExecuteChanged));
            if (expectedChangedCount < 0) throw new ArgumentOutOfRangeException(nameof(expectedChangedCount), "A negative number is not allowed.");
            
            int canExecuteChangedCount = 0;

            void CanExecuteChangedHandler(object? sender, EventArgs e)
            {
                if (command != sender) throw new AssertException("The sender object of the event isn't the command");
                canExecuteChangedCount++;
            }

            command.CanExecuteChanged += CanExecuteChangedHandler;
            raiseCanExecuteChanged();
            command.CanExecuteChanged -= CanExecuteChangedHandler;

            if (canExecuteChangedCount < expectedChangedCount && expectedChangedCountMode != ExpectedChangedCountMode.AtMost)
            {
                throw new AssertException(string.Format(null, "The CanExecuteChanged event was raised {0} times. Expected is {1} times{2}.", 
                    canExecuteChangedCount, expectedChangedCount,
                    expectedChangedCountMode == ExpectedChangedCountMode.AtLeast ? " or more" : ""));
            }
            else if (canExecuteChangedCount > expectedChangedCount && expectedChangedCountMode != ExpectedChangedCountMode.AtLeast)
            {
                throw new AssertException(string.Format(null, "The CanExecuteChanged event was raised {0} times. Expected is {1} times{2}.",
                    canExecuteChangedCount, expectedChangedCount,
                    expectedChangedCountMode == ExpectedChangedCountMode.AtMost ? " or less" : ""));
            }
        }

        /// <summary>Asserts that the execution of the provided action raises the property changed event.</summary>
        /// <typeparam name="T">The type of the observable.</typeparam>
        /// <param name="observable">The observable which should raise the property changed event.</param>
        /// <param name="expression">A simple expression which identifies the property (e.g. x => x.Name).</param>
        /// <param name="raisePropertyChanged">An action that results in a property changed event of the observable.</param>
        /// <exception cref="AssertException">This exception is thrown when no or more than one property changed event was 
        /// raised by the observable or the sender object of the event was not the observable object.</exception>
        public static void PropertyChangedEvent<T>(T observable, Expression<Func<T, object?>> expression, Action raisePropertyChanged) where T : class, INotifyPropertyChanged
        {
            PropertyChangedEvent(observable, expression, raisePropertyChanged, 1, ExpectedChangedCountMode.Exact);
        }

        /// <summary>Asserts that the execution of the provided action raises the property changed event.</summary>
        /// <typeparam name="T">The type of the observable.</typeparam>
        /// <param name="observable">The observable which should raise the property changed event.</param>
        /// <param name="expression">A simple expression which identifies the property (e.g. x => x.Name).</param>
        /// <param name="raisePropertyChanged">An action that results in a property changed event of the observable.</param>
        /// <param name="expectedChangedCount">The expected count of PropertyChanged events.</param>
        /// <param name="expectedChangedCountMode">The mode defines how the expected changed count is used as assert condition.</param>
        /// <exception cref="AssertException">This exception is thrown when no or more than one property changed event was 
        /// raised by the observable or the sender object of the event was not the observable object.</exception>
        public static void PropertyChangedEvent<T>(T observable, Expression<Func<T, object?>> expression, Action raisePropertyChanged, int expectedChangedCount,
            ExpectedChangedCountMode expectedChangedCountMode) where T : class, INotifyPropertyChanged
        {
            if (observable == null) throw new ArgumentNullException(nameof(observable));
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            if (raisePropertyChanged == null) throw new ArgumentNullException(nameof(raisePropertyChanged));
            if (expectedChangedCount < 0) throw new ArgumentOutOfRangeException(nameof(expectedChangedCount), "A negative number is not allowed.");
            
            string propertyName = GetProperty(expression).Name;
            int propertyChangedCount = 0;

            void PropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
            {
                if (observable != sender) throw new AssertException("The sender object of the event isn't the observable");
                if (e.PropertyName == propertyName) propertyChangedCount++;
            }

            observable.PropertyChanged += PropertyChangedHandler;
            raisePropertyChanged();
            observable.PropertyChanged -= PropertyChangedHandler;

            if (propertyChangedCount < expectedChangedCount && expectedChangedCountMode != ExpectedChangedCountMode.AtMost)
            {
                throw new AssertException(string.Format(null, "The PropertyChanged event for the property '{0}' was raised {1} times. Expected is {2} times{3}.",
                    propertyName, propertyChangedCount, expectedChangedCount,
                    expectedChangedCountMode == ExpectedChangedCountMode.AtLeast ? " or more" : ""));
            }
            else if (propertyChangedCount > expectedChangedCount && expectedChangedCountMode != ExpectedChangedCountMode.AtLeast)
            {
                throw new AssertException(string.Format(null, "The PropertyChanged event for the property '{0}' was raised {1} times. Expected is {2} times{3}.", 
                    propertyName, propertyChangedCount, expectedChangedCount,
                    expectedChangedCountMode == ExpectedChangedCountMode.AtMost ? " or less" : ""));
            }
        }

        /// <summary>Verifies that two sequences are equal by comparing their elements. The assertion fails if the sequences are not equal.</summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <param name="expected">An System.Collections.Generic.IEnumerable`1 to compare to the actual sequence.</param>
        /// <param name="actual">An System.Collections.Generic.IEnumerable`1 to compare to the expected sequence.</param>
        /// <exception cref="ArgumentNullException">expected or actual is null.</exception>
        /// <exception cref="AssertException">expected is not equal to actual.</exception>
        public static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual) => SequenceEqual(expected, actual, null);

        /// <summary>
        /// Verifies that two sequences are equal by comparing their elements by using a specified System.Collections.Generic.IEqualityComparer`1. 
        /// The assertion fails if the sequences are not equal.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <param name="expected">An System.Collections.Generic.IEnumerable`1 to compare to the actual sequence.</param>
        /// <param name="actual">An System.Collections.Generic.IEnumerable`1 to compare to the expected sequence.</param>
        /// <param name="comparer">An System.Collections.Generic.IEqualityComparer`1 to use to compare elements.</param>
        /// <exception cref="ArgumentNullException">expected or actual is null.</exception>
        /// <exception cref="AssertException">expected is not equal to actual.</exception>
        public static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? comparer)
        {
            if (expected == null) throw new ArgumentNullException(nameof(expected));
            if (actual == null) throw new ArgumentNullException(nameof(actual));
            if (!expected.SequenceEqual(actual, comparer))
            {
                throw new AssertException(string.Format(null, "The actual sequence is not equal to the expected sequence." + Environment.NewLine
                    + "actual:   count: {0}; items (Take 20): [{1}]" + Environment.NewLine
                    + "expected: count: {2}; items (Take 20): [{3}]",
                    actual.Count(), string.Join(", ", actual.Take(20)), expected.Count(), string.Join(", ", expected.Take(20))));
            }
        }

        internal static PropertyInfo GetProperty<T>(Expression<Func<T, object?>> propertyExpression)
        {
            Expression expression = propertyExpression.Body;

            // If the Property returns a ValueType then a Convert is required => Remove it
            if (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
            {
                expression = ((UnaryExpression)expression).Operand;
            }

            // If this isn't a member access expression then the expression isn't valid
            var memberExpression = expression as MemberExpression ?? throw CreateExpressionArgumentException();
            
            expression = memberExpression!.Expression ?? throw CreateExpressionArgumentException();

            // If the Property returns a ValueType then a Convert is required => Remove it
            if (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
            {
                expression = ((UnaryExpression)expression).Operand;
            }

            // Check if the expression is the parameter itself
            if (expression.NodeType != ExpressionType.Parameter) throw CreateExpressionArgumentException();

            // Finally retrieve the PropertyInfo
            var propertyInfo = memberExpression.Member as PropertyInfo ?? throw CreateExpressionArgumentException();
            return propertyInfo;

            static ArgumentException CreateExpressionArgumentException() => new ArgumentException("Only the simple expression 'x => x.[Property]' is allowed. "
                    + "[Property] must be replaced with the property name that should be used.", "propertyExpression");
        }
    }
}
