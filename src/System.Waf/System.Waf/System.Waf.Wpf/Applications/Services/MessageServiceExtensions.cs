using System.Globalization;

namespace System.Waf.Applications.Services
{
    /// <summary>Provides method overloads for the <see cref="IMessageService"/> to simplify its usage.</summary>
    public static class MessageServiceExtensions
    {
        /// <summary>Shows the message.</summary>
        /// <param name="service">The message service.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">The argument service must not be null.</exception>
        public static void ShowMessage(this IMessageService service, string message) => (service ?? throw new ArgumentNullException(nameof(service))).ShowMessage(null, message);

        /// <summary>Shows the message.</summary>
        /// <param name="service">The message service.</param>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException">The argument service must not be null.</exception>
        public static void ShowMessage(this IMessageService service, object? owner, string format, params object?[] args) => (service ?? throw new ArgumentNullException(nameof(service))).ShowMessage(owner, SF(format, args));

        /// <summary>Shows the message as warning.</summary>
        /// <param name="service">The message service.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">The argument service must not be null.</exception>
        public static void ShowWarning(this IMessageService service, string message) => (service ?? throw new ArgumentNullException(nameof(service))).ShowWarning(null, message);

        /// <summary>Shows the message as warning.</summary>
        /// <param name="service">The message service.</param>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException">The argument service must not be null.</exception>
        public static void ShowWarning(this IMessageService service, object? owner, string format, params object?[] args) => (service ?? throw new ArgumentNullException(nameof(service))).ShowWarning(owner, SF(format, args));

        /// <summary>Shows the message as error.</summary>
        /// <param name="service">The message service.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">The argument service must not be null.</exception>
        public static void ShowError(this IMessageService service, string message) => (service ?? throw new ArgumentNullException(nameof(service))).ShowError(null, message);

        /// <summary>Shows the message as error.</summary>
        /// <param name="service">The message service.</param>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <exception cref="ArgumentNullException">The argument service must not be null.</exception>
        public static void ShowError(this IMessageService service, object? owner, string format, params object?[] args) => (service ?? throw new ArgumentNullException(nameof(service))).ShowError(owner, SF(format, args));

        /// <summary>Shows the specified question.</summary>
        /// <param name="service">The message service.</param>
        /// <param name="message">The question.</param>
        /// <returns><c>true</c> for yes, <c>false</c> for no and <c>null</c> for cancel.</returns>
        /// <exception cref="ArgumentNullException">The argument service must not be null.</exception>
        public static bool? ShowQuestion(this IMessageService service, string message) => (service ?? throw new ArgumentNullException(nameof(service))).ShowQuestion(null, message);

        /// <summary>Shows the specified question.</summary>
        /// <param name="service">The message service.</param>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns><c>true</c> for yes, <c>false</c> for no and <c>null</c> for cancel.</returns>
        /// <exception cref="ArgumentNullException">The argument service must not be null.</exception>
        public static bool? ShowQuestion(this IMessageService service, object? owner, string format, params object?[] args) => (service ?? throw new ArgumentNullException(nameof(service))).ShowQuestion(owner, SF(format, args));

        /// <summary>Shows the specified yes/no question.</summary>
        /// <param name="service">The message service.</param>
        /// <param name="message">The question.</param>
        /// <returns><c>true</c> for yes and <c>false</c> for no.</returns>
        /// <exception cref="ArgumentNullException">The argument service must not be null.</exception>
        public static bool ShowYesNoQuestion(this IMessageService service, string message) => (service ?? throw new ArgumentNullException(nameof(service))).ShowYesNoQuestion(null, message);

        /// <summary>Shows the specified yes/no question.</summary>
        /// <param name="service">The message service.</param>
        /// <param name="owner">The window that owns this Message Window.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns><c>true</c> for yes and <c>false</c> for no.</returns>
        /// <exception cref="ArgumentNullException">The argument service must not be null.</exception>
        public static bool ShowYesNoQuestion(this IMessageService service, object? owner, string format, params object?[] args) => (service ?? throw new ArgumentNullException(nameof(service))).ShowYesNoQuestion(owner, SF(format, args));

        private static string SF(string format, params object?[] args) => string.Format(CultureInfo.CurrentCulture, format, args);
    }
}
