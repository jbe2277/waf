using System;

namespace Waf.InformationManager.Infrastructure.Interfaces.Applications
{
    /// <summary>
    /// Provides application settings.
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Register the classes which are used as application settings.
        /// </summary>
        /// <param name="types">The classes to be used as application settings.</param>
        void RegisterTypes(params Type[] types);

        /// <summary>
        /// Gets the specified application settings instance.
        /// </summary>
        /// <typeparam name="T">The application settings class.</typeparam>
        /// <returns>The application settings instance.</returns>
        T Get<T>() where T : class, new();

        /// <summary>
        /// Saves all application instances.
        /// </summary>
        void Save();
    }
}
