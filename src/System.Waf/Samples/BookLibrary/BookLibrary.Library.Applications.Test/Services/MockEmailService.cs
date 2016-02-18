using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Waf.BookLibrary.Library.Applications.Services;
using System.ComponentModel.Composition;

namespace Test.BookLibrary.Library.Applications.Services
{
    [Export(typeof(IEmailService)), Export]
    internal class MockEmailService : IEmailService
    {
        public string ToEmailAddress { get; set; }
        

        public void CreateNewEmail(string toEmailAddress)
        {
            ToEmailAddress = toEmailAddress;
        }
    }
}
