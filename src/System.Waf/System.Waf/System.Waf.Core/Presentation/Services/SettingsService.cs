using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Xml;
using System.Xml.Linq;

namespace System.Waf.Presentation.Services
{
    /// <summary>Service that is responsible to load and save user settings.</summary>
    public class SettingsService : ISettingsService, IDisposable
    {
        private static readonly XNamespace xsiNamespace = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
        private static readonly XNamespace dcsArrayNamespace = XNamespace.Get("http://schemas.microsoft.com/2003/10/Serialization/Arrays");
        private static readonly XNamespace dcNamespace = XNamespace.Get("http://schemas.datacontract.org/2004/07/");

        private readonly ConcurrentDictionary<Type, Lazy<object>> settings = new();
        private string fileName;
        private int isDisposed;

        /// <summary>Initializes a new instance of the SettingsService.</summary>
        public SettingsService()
        {
            fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ApplicationInfo.ProductName, "Settings", "Settings.xml");
        }

        /// <inheritdoc />
        public string FileName
        {
            get => fileName;
            set
            {
                if (settings.Any()) throw new InvalidOperationException("Setter must not be called anymore if one of the methods was used before.");
                fileName = value;
            }
        }

        /// <inheritdoc />
        public event EventHandler<SettingsErrorEventArgs>? ErrorOccurred;

        /// <inheritdoc />
        public T Get<T>() where T : class, new() => (T)settings.GetOrAdd(typeof(T), key => new(() => Load(key))).Value;

        /// <inheritdoc />
        public void Save()
        {
            var settingsList = settings.Values.Select(x => x.Value).ToArray();
            if (!settingsList.Any()) return;
            try
            {
                XDocument? document = null;
                if (File.Exists(FileName))
                {
                    try
                    {
                        using var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                        document = XDocument.Load(stream);
                    }
                    catch (Exception ex)
                    {
                        OnErrorOccurred(new(ex, SettingsServiceAction.Save, FileName));
                    }
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(FileName) ?? throw new DirectoryNotFoundException("Directory not found: " + FileName));
                }

                document ??= CreateEmptyDocument();
                try
                {
                    UpdateDocument(document, settingsList);
                }
                catch (Exception ex)
                {
                    OnErrorOccurred(new(ex, SettingsServiceAction.Save, FileName));

                    document = CreateEmptyDocument();
                    UpdateDocument(document, settingsList);
                }

                using (var stream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    document.Save(stream);
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new(ex, SettingsServiceAction.Save, FileName));
            }
        }

        private static void UpdateDocument(XDocument document, IReadOnlyList<object> settingsList)
        {
            foreach (var setting in settingsList)
            {
                var existingElement = GetSettingElement(document, setting.GetType());
                var newElement = CreateSettingElement(setting);
                if (existingElement != null) existingElement.ReplaceWith(newElement);
                else document.Root!.Add(newElement);
            }
        }

        /// <summary>Calls save to ensure that the latest changes are persisted.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Call this method from a subclass when you implement a finalizer (destructor).</summary>
        /// <param name="disposing">if true then dispose unmanaged and managed resources; otherwise dispose only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) != 0) return;
            OnDispose(disposing);
            if (disposing) Save();
        }

        /// <summary>Override this method to free, release or reset any resources.</summary>
        /// <param name="disposing">if true then dispose unmanaged and managed resources; otherwise dispose only unmanaged resources.</param>
        protected virtual void OnDispose(bool disposing) { }

        private void OnErrorOccurred(SettingsErrorEventArgs e) => ErrorOccurred?.Invoke(this, e);

        private object Load(Type type)
        {
            object? settingObject = null;
            try
            {
                settingObject = LoadCore(type);
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new(ex, SettingsServiceAction.Open, FileName));
            }
            return settingObject ?? Activator.CreateInstance(type)!;  // type has default ctor
        }

        private object? LoadCore(Type type)
        {
            if (!File.Exists(FileName)) return null;

            using var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var element = GetSettingElement(XDocument.Load(stream), type);
            if (element != null)
            {
                var serializer = new DataContractSerializer(typeof(List<object>), new[] { type });
                var document = CreateEmptyDocument();
                document.Root!.Add(element);
                using var memoryStream = new MemoryStream();
                document.Save(memoryStream);
                memoryStream.Position = 0;
                var settingObject = serializer.ReadObject(memoryStream);
                return ((List<object>)settingObject!).Single();
            }
            return null;
        }

        private static XDocument CreateEmptyDocument()
        {
            var document = new XDocument();
            document.Add(new XElement(dcsArrayNamespace + "ArrayOfanyType", new XAttribute(XNamespace.Xmlns + "i", xsiNamespace)));
            return document;
        }

        private static XElement? GetSettingElement(XDocument document, Type settingType)
        {
            var dataContract = settingType.GetCustomAttribute<DataContractAttribute>();
            var settingTypeNamespace = dataContract?.Namespace ?? dcNamespace.NamespaceName + settingType.Namespace;
            var settingTypeName = dataContract?.Name ?? GetTypeName(settingType);

            foreach (var element in document.Root!.Elements())
            {
                var typeValue = (element.Attribute(xsiNamespace + "type")?.Value.Split(new[] { ':' }, 2)) ?? throw new XmlException("Wrong XML format. Cannot read type.");
                var typeNamespace = (element.Attribute(XNamespace.Xmlns + typeValue[0])?.Value) ?? throw new XmlException("Wrong XML format. Cannot read type namespace.");
                var typeName = typeValue[1];
                if (settingTypeNamespace == typeNamespace && settingTypeName == typeName) return element;
            }
            return null;
        }

        private static string GetTypeName(Type type)
        {
            string? name = null;
            while (type.IsNested)
            {
                name = name == null ? type.Name : type.Name + "." + name;
                type = type.DeclaringType!;
            }
            name = name == null ? type.Name : type.Name + "." + name;
            return name;
        }

        private static XElement CreateSettingElement(object setting)
        {
            var document = new XDocument();
            using (var writer = document.CreateWriter())
            {
                var dcs = new DataContractSerializer(typeof(List<object>), new[] { setting.GetType() });
                dcs.WriteObject(writer, new List<object> { setting });
            }
            return document.Root!.Elements().Single();
        }
    }
}
