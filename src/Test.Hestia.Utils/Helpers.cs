using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Test.Hestia.Utils
{
    public static class Helpers
    {
        public static string LoadResource(string name, Assembly assembly = null)
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

            using var stream = assembly.GetManifestResourceStream(resourcePath);
            using var reader = new StreamReader(stream ?? throw new Exception());

            return reader.ReadToEnd();
        }
    }
}
