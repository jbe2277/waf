using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.InformationManager.AddressBook.Modules.Domain;
using Waf.InformationManager.AddressBook.Modules.Applications;

namespace Test.InformationManager.AddressBook.Modules.Applications;

[TestClass]
public class DtoFactoryTest
{
    [TestMethod]
    public void ToDtoTest()
    {
        var contact = new Contact() { Firstname = "Jesper", Lastname = "Aaberg", Email = "j.a@example.com" };

        var contactDto = contact.ToDto();

        Assert.AreEqual(contact.Firstname, contactDto.Firstname);
        Assert.AreEqual(contact.Lastname, contactDto.Lastname);
        Assert.AreEqual(contact.Email, contactDto.Email);

        Assert.IsNull(DtoFactory.ToDto(null));
    }
}
