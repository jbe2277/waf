using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Waf.Foundation;
using Waf.BookLibrary.Library.Domain.Properties;

namespace Waf.BookLibrary.Library.Domain
{
    [DebuggerDisplay("Person: {Firstname} {Lastname}")]
    public class Person : ValidatableModel, IFormattable
    {
        private string firstname;
        private string lastname;
        private string email;


        public Person()
        {
            Id = Guid.NewGuid();
            firstname = "";
            lastname = "";
        }


        public Guid Id { get; private set; }

        [Required(ErrorMessageResourceName = "FirstnameMandatory", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(30, ErrorMessageResourceName = "FirstnameMaxLength", ErrorMessageResourceType = typeof(Resources))]
        public string Firstname 
        {
            get { return firstname; }
            set { SetPropertyAndValidate(ref firstname, value); }
        }

        [Required(ErrorMessageResourceName = "LastnameMandatory", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(30, ErrorMessageResourceName = "LastnameMaxLength", ErrorMessageResourceType = typeof(Resources))]
        public string Lastname 
        {
            get { return lastname; }
            set { SetPropertyAndValidate(ref lastname, value); }
        }

        [StringLength(100, ErrorMessageResourceName = "EmailMaxLength", ErrorMessageResourceType = typeof(Resources))]
        [EmailAddress(ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resources))]
        public string Email 
        {
            get { return email; }
            set { SetPropertyAndValidate(ref email, value == "" ? null : value); }
        }


        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format(formatProvider, Resources.PersonToString, Firstname, Lastname);
        }
    }
}
