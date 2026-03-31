using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.IO
{
    public struct FileChange
    {

        public FileChangeType ChangeType { get; set; }
        public IFile File { get; set; }
        public string? OldFileFullPath { get; set; }

    }
}
