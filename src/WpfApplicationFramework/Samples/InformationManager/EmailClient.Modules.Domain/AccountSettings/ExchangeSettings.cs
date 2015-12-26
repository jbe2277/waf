using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings
{
    [DataContract]
    public class ExchangeSettings : EmailAccountSettings
    {
        [DataMember] private string serverPath;
        [DataMember] private string userName;


        [Required, Display(Name = "Exchange Server")]
        public string ServerPath
        {
            get { return serverPath; }
            set { SetProperty(ref serverPath, value); }
        }

        [Required, Display(Name = "User Name")]
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }


        public override EmailAccountSettings Clone()
        {
            return new ExchangeSettings() { serverPath = this.serverPath, userName = this.userName };
        }
    }
}
