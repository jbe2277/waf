using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings
{
    [DataContract]
    public class UserCredits : ValidatableModel
    {
        [DataMember] private string? userName;
        [DataMember] private string? password;

        [Required, Display(Name = "Username")]
        public string? UserName
        {
            get => userName;
            set => SetPropertyAndValidate(ref userName, value);
        }

        public string? Password
        {
            get => password;
            set => SetPropertyAndValidate(ref password, value);
        }

        public virtual UserCredits Clone()
        {
            var clone = new UserCredits() { userName = userName, password = password };
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
