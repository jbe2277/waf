using Jbe.NewsReader.Applications.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Composition.Hosting;
using System.Reflection;
using Test.NewsReader.Domain.UnitTesting;

namespace Test.NewsReader.Applications.UnitTesting
{
    [TestClass]
    public abstract class ApplicationsTest : DomainTest
    {
        protected CompositionHost Container { get; private set; }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            var configuration = new ContainerConfiguration()
                .WithAssembly(typeof(ShellViewModel).GetTypeInfo().Assembly)
                .WithAssembly(typeof(ApplicationsTest).GetTypeInfo().Assembly);
            Container = configuration.CreateContainer();
        }
    }
}
