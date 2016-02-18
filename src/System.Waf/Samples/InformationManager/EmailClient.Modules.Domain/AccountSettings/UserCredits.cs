using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Waf.InformationManager.Common.Domain;

namespace Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings
{
    [DataContract]
    public class UserCredits : ValidationModel
    {
        [DataMember] private string userName;
        [DataMember] private string password;


        [Required, Display(Name = "Username")]
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }


        public virtual UserCredits Clone()
        {
            return new UserCredits() { userName = this.userName, password = this.password };
        }
    }
}
