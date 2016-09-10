using Jbe.NewsReader.Applications.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Composition.Hosting;
using System.Reflection;
using System.Waf.UnitTesting;
using Test.NewsReader.Domain.UnitTesting;

namespace Test.NewsReader.Applications.UnitTesting
{
    [TestClass]
    public abstract class ApplicationsTest : DomainTest
    {
        protected UnitTestSynchronizationContext Context { get; private set; }

        protected CompositionHost Container { get; private set; }


        protected override void OnInitialize()
        {
            base.OnInitialize();

            Context = UnitTestSynchronizationContext.Create();

            var configuration = new ContainerConfiguration()
                .WithAssembly(typeof(ShellViewModel).GetTypeInfo().Assembly)
                .WithAssembly(typeof(ApplicationsTest).GetTypeInfo().Assembly);
            Container = configuration.CreateContainer();
        }

        protected override void OnCleanup()
        {
            Container.Dispose();
            Context.Dispose();

            base.OnCleanup();
        }
    }
}
