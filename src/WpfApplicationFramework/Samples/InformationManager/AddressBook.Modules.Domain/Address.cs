using System.Runtime.Serialization;
using Waf.InformationManager.Common.Domain;

namespace Waf.InformationManager.AddressBook.Modules.Domain
{
    [DataContract]
    public class Address : ValidationModel
    {
        [DataMember] private string street;
        [DataMember] private string city;
        [DataMember] private string state;
        [DataMember] private string postalCode;
        [DataMember] private string country;


        public string Street
        {
            get { return street; }
            set { SetProperty(ref street, value); }
        }

        public string City
        {
            get { return city; }
            set { SetProperty(ref city, value); }
        }

        public string State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }

        public string PostalCode
        {
            get { return postalCode; }
            set { SetProperty(ref postalCode, value); }
        }

        public string Country
        {
            get { return country; }
            set { SetProperty(ref country, value); }
        }
    }
}
