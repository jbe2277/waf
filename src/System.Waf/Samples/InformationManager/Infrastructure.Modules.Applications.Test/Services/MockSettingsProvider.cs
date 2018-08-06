using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Test.InformationManager.Infrastructure.Modules.Applications.Services
{
    [Export(typeof(ISettingsProvider)), Export]
    public class MockSettingsProvider : ISettingsProvider
    {
        private readonly List<object> settings = new List<object>();

        public List<Type> RegisteredTypes { get; } = new List<Type>();

        public Func<Type, object> GetStub { get; set; }

        public void RegisterTypes(params Type[] types)
        {
            RegisteredTypes.AddRange(types);
        }

        public T Get<T>() where T : class, new()
        {
            if (GetStub != null) return (T)GetStub.Invoke(typeof(T));
            var instance = settings.OfType<T>().SingleOrDefault();
            if (instance == null)
            {
                instance = new T();
                settings.Add(instance);
            }
            return instance;
        }

        public void Save()
        {
        }
    }
}
