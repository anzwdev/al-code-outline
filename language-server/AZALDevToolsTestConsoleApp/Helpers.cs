using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsTestConsoleApp
{
    internal static class Helpers
    {

        public static void CopyFolder(string source, string destination)
        {
            var files = Directory.GetFiles(source);
            foreach (var srcFile in files)
            {
                var name = Path.GetFileName(srcFile);
                var destFile = Path.Combine(destination, name);
                File.Copy(srcFile, destFile, true);
            }

            var directories = Directory.GetDirectories(source);
            foreach (var srcDir in directories) 
            {
                var name = Path.GetFileName(srcDir);
                var destDir = Path.Combine(destination, name);
                CopyFolder(srcDir, destDir);
            }
        }

    }
}
