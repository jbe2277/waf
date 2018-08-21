using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Waf.Applications;
using Waf.InformationManager.Infrastructure.Interfaces.Applications;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation.Services
{
    [Export(typeof(ISettingsProvider))]
    internal class SettingsProvider : ISettingsProvider, IDisposable
    {
        private const string settingsFolder = "Settings";
        private const string settingsFile = "Settings.xml";

        private readonly Lazy<string> settingsPath;
        private readonly List<Type> knownTypes;
        private readonly Lazy<List<object>> settings;

        public SettingsProvider()
        {
            settingsPath = new Lazy<string>(() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                ApplicationInfo.ProductName, settingsFolder, settingsFile));
            knownTypes = new List<Type>();
            settings = new Lazy<List<object>>(LoadCore);
        }

        public void RegisterTypes(params Type[] types)
        {
            knownTypes.AddRange(types);
        }

        public T Get<T>() where T : class, new()
        {
            var instance = settings.Value.OfType<T>().SingleOrDefault();
            if (instance == null)
            {
                instance = new T();
                settings.Value.Add(instance);
            }
            return instance;
        }

        public void Save()
        {
            if (!settings.IsValueCreated) return;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(settingsPath.Value));
                using (var stream = new FileStream(settingsPath.Value, FileMode.Create, FileAccess.Write))
                {
                    var serializer = new DataContractSerializer(typeof(List<object>), knownTypes);
                    serializer.WriteObject(stream, settings.Value);
                }
            }
            catch (Exception ex)
            {
                Log.Default.Error(ex, "SettingsProvider save");
            }
        }

        private List<object> LoadCore()
        {
            try
            {
                if (!File.Exists(settingsPath.Value)) return new List<object>();

                using (var stream = new FileStream(settingsPath.Value, FileMode.Open, FileAccess.Read))
                {
                    var serializer = new DataContractSerializer(typeof(List<object>), knownTypes);
                    return (List<object>)serializer.ReadObject(stream);
                }
            }
            catch (Exception ex)
            {
                Log.Default.Error(ex, "SettingsProvider load");
                return new List<object>();
            }
        }

        public void Dispose()
        {
            Save();
        }
    }
}
