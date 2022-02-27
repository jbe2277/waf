using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Test.InformationManager.Common.Domain;

[TestClass]
public abstract class DomainTest
{
    [TestInitialize]
    public void Initialize()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        OnInitialize();
    }

    [TestCleanup]
    public void Cleanup() => OnCleanup();

    protected virtual void OnInitialize() { }

    protected virtual void OnCleanup() { }
}
