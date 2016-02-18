using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Waf.InformationManager.Common.Domain;

namespace Waf.InformationManager.AddressBook.Modules.Domain
{
    [DataContract]
    public class Contact : ValidationModel
    {
        [DataMember] private readonly Address address;
        [DataMember] private string firstname;
        [DataMember] private string lastname;
        [DataMember] private string company;
        [DataMember] private string email;
        [DataMember] private string phone;


        public Contact()
        {
            address = new Address();
        }


        public string Firstname
        {
            get { return firstname; }
            set { SetProperty(ref firstname, value); }
        }

        public string Lastname
        {
            get { return lastname; }
            set { SetProperty(ref lastname, value); }
        }

        public string Company
        {
            get { return company; }
            set { SetProperty(ref company, value); }
        }

        [EmailAddress]
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        public string Phone
        {
            get { return phone; }
            set { SetProperty(ref phone, value); }
        }

        public Address Address => address;
    }
}
