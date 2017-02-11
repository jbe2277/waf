using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;

namespace Test.NewsReader.Applications.Services
{
    [Export(typeof(IResourceService)), Export, Shared]
    public class MockResourceService : IResourceService
    {
        public Func<string, string> GetStringStub { get; set; }


        public string GetString(string resource, params object[] args)
        {
            return GetStringStub?.Invoke(resource) ?? "Dummy Text";
        }
    }
}
