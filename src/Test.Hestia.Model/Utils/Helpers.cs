using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Test.Hestia.Model.Utils
{
    public static class Helpers
    {
        public static string LoadResource(string name, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }

            string resourcePath = name;

            // ReSharper disable once RedundantNameQualifier
            if (!name.StartsWith(nameof(Test.Hestia.Model.Resources)))
            {
                resourcePath = assembly.GetManifestResourceNames()
                                       .Single(str => str.EndsWith(name));
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream ?? throw new Exception()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
