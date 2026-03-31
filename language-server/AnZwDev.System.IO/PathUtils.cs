using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.IO
{
    public static class PathUtils
    {

        public static string NormalizePath(string path)
        {
            return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

        public static StringComparison GetPathComparison()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;
        }

        public static IEqualityComparer<string> GetPathComparer()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? StringComparer.OrdinalIgnoreCase
                : StringComparer.Ordinal;
        }

        public static bool ContainsPath(string basePath, string path)
        {
            basePath = NormalizePath(basePath).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            path = NormalizePath(path).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            var comparison = GetPathComparison();
            return path.StartsWith(basePath, comparison);
        }

        public static bool Equals(string path1, string path2)
        {
            path1 = NormalizePath(path1).TrimEnd(Path.DirectorySeparatorChar);
            path2 = NormalizePath(path2).TrimEnd(Path.DirectorySeparatorChar);

            var comparison = GetPathComparison();
            return string.Equals(path1, path2, comparison);
        }

        public static string Combine(Assembly assembly, string fileName)
        {
            var mainPath = Path.GetFullPath(Path.GetDirectoryName(assembly.Location) ?? "");
            return Path.Combine(mainPath, fileName);
        }

    }
}
