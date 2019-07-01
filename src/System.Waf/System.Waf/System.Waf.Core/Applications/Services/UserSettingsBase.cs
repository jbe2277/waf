using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace System.Waf.Applications.Services
{
    /// <summary>
    /// Base class for user settings.
    /// </summary>
    [DataContract]
    public abstract class UserSettingsBase : Model, IExtensibleDataObject
    {
        /// <summary>
        /// Initializes a new instance of the UserSettingsBase.
        /// </summary>
        protected UserSettingsBase()
        {
            SetDefaultValues();  // The call of the virtual method is correct in this case.
        }

        ExtensionDataObject IExtensibleDataObject.ExtensionData { get; set; }

        /// <summary>
        /// Set the default values within this method. During deserialization the constructor is NOT called
        /// but this method is.
        /// </summary>
        protected abstract void SetDefaultValues();

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            SetDefaultValues();
        }
    }
}
