using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace AnZwDev.ALTools.Core
{
    public static class PathUtils
    {

        public static bool ContainsPath(string parentPath, string path)
        {
            return ((path.StartsWith(parentPath + Path.DirectorySeparatorChar, GetPathComparison())) || (path.Equals(parentPath, GetPathComparison())));
        }

        public static string GetRelativePath(string parentPath, string fullPath)
        {
            if (fullPath.StartsWith(parentPath, GetPathComparison()))
            {
                string relativePath = fullPath.Substring(parentPath.Length);
                if ((relativePath.Length > 0) && (relativePath[0] == Path.DirectorySeparatorChar))
                {
                    return relativePath.Substring(1);
                }
            }
            return null;
        }

        public static StringComparison GetPathComparison()
        {
#if BC
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return StringComparison.CurrentCultureIgnoreCase;
            return StringComparison.CurrentCulture;
#else
            return StringComparison.CurrentCultureIgnoreCase;
#endif
        }


    }
}
