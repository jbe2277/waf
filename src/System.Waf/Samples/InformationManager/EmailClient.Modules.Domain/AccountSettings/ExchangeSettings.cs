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
            set { SetPropertyAndValidate(ref serverPath, value); }
        }

        [Required, Display(Name = "User Name")]
        public string UserName
        {
            get { return userName; }
            set { SetPropertyAndValidate(ref userName, value); }
        }


        public override EmailAccountSettings Clone()
        {
            var clone = new ExchangeSettings() { serverPath = serverPath, userName = userName };
            clone.Validate();
            return clone;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Validate();
        }
    }
}
