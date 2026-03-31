using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.IO
{
    public class FileSystemFile : IFile
    {
        public Encoding Encoding { get; set; }

        public string FullPath { get; }

        public FileSystemFile(string fullPath) : this(fullPath, Encoding.UTF8)
        {
        }

        public FileSystemFile(string fullPath, Encoding encoding)
        {
            FullPath = fullPath;
            Encoding = encoding;
        }

        public string ReadAllText()
        {
            return FileHelper.ReadAllTextWithRetry(this.FullPath, this.Encoding);
        }

        public void WriteAllText(string content)
        {
            File.WriteAllText(this.FullPath, content, this.Encoding);
        }

    }
}
