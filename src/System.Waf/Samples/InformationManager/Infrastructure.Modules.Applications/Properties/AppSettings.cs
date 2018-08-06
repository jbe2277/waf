using System.Runtime.Serialization;

namespace Waf.InformationManager.Infrastructure.Modules.Applications.Properties
{
    [DataContract]
    public sealed class AppSettings : IExtensibleDataObject
    {
        public AppSettings()
        {
            SetDefaultValues();
        }

        [DataMember] public double Left { get; set; }

        [DataMember] public double Top { get; set; }

        [DataMember] public double Height { get; set; }

        [DataMember] public double Width { get; set; }

        [DataMember] public bool IsMaximized { get; set; }

        ExtensionDataObject IExtensibleDataObject.ExtensionData { get; set; }

        private void SetDefaultValues()
        {
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            SetDefaultValues();
        }
    }
}
