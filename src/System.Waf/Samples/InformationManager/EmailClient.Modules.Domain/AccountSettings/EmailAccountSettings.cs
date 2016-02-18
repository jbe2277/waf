using System.Runtime.Serialization;
using Waf.InformationManager.Common.Domain;

namespace Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings
{
    [DataContract]
    public abstract class EmailAccountSettings : ValidationModel
    {
        public abstract EmailAccountSettings Clone();
    }
}
