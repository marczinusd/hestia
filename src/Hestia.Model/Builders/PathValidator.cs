using System;

namespace Hestia.Model.Builders
{
    public class PathValidator : IPathValidator
    {
        public void ValidateDirectoryPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Root path cannot be null or empty", nameof(path));
            }

            if (!System.IO.Directory.Exists(path))
            {
                throw new
                    InvalidOperationException($"Repository cannot be built because the path ({path}) provided is invalid. ");
            }
        }
    }
}
