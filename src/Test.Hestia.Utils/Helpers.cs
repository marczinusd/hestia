using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;

namespace Test.Hestia.Utils
{
    [ExcludeFromCodeCoverage]
    public static class Helpers
    {
        public static string LoadResource(string name, Assembly assembly = null)
        {
            using var stream = LoadResourceStream(name, assembly);
            using var reader = new StreamReader(stream ?? throw new Exception());

            return reader.ReadToEnd();
        }

        public static T LoadAndDeserializeXmlResource<T>(string name, Assembly assembly = null)
        {
            using var stream = LoadResourceStream(name, assembly);
            var serializer = new XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(stream);
        }

        public static void After(TimeSpan timeSpan, Action actionToPerform)
        {
            Thread.Sleep(timeSpan);
            actionToPerform();
        }

        private static Stream LoadResourceStream(string name, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }

            var resourcePath = name;

            // ReSharper disable once RedundantNameQualifier
            if (!name.StartsWith(nameof(Test.Hestia.Utils)))
            {
                resourcePath = assembly.GetManifestResourceNames()
                                       .Single(str => str.EndsWith(name));
            }

            return assembly.GetManifestResourceStream(resourcePath);
        }
    }
}
