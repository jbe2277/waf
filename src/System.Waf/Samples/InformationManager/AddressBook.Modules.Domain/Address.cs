using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Waf.InformationManager.AddressBook.Modules.Domain
{
    [DataContract]
    public class Address : ValidatableModel
    {
        [DataMember] private string street;
        [DataMember] private string city;
        [DataMember] private string state;
        [DataMember] private string postalCode;
        [DataMember] private string country;


        public string Street
        {
            get { return street; }
            set { SetPropertyAndValidate(ref street, value); }
        }

        public string City
        {
            get { return city; }
            set { SetPropertyAndValidate(ref city, value); }
        }

        public string State
        {
            get { return state; }
            set { SetPropertyAndValidate(ref state, value); }
        }

        public string PostalCode
        {
            get { return postalCode; }
            set { SetPropertyAndValidate(ref postalCode, value); }
        }

        public string Country
        {
            get { return country; }
            set { SetPropertyAndValidate(ref country, value); }
        }
    }
}
