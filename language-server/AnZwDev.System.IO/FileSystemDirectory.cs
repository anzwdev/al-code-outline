using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.IO
{
    public class FileSystemDirectory
    {

        public static List<IFile> GetFiles(string path, string searchPattern)
        {
            return GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public static List<IFile> GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            var filePaths = Directory.GetFiles(path, searchPattern, searchOption);

            var files = new List<IFile>(filePaths.Length);
            for (int i = 0; i < filePaths.Length; i++)
                files.Add(new FileSystemFile(filePaths[i]));

            return files;
        }

    }
}
