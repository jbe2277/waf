using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;

[DataContract]
public abstract class EmailAccountSettings : ValidatableModel
{
    public abstract EmailAccountSettings Clone();
}
