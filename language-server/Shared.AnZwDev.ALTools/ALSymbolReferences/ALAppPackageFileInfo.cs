using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using AnZwDev.ALTools.Core;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPackageFileInfo
    {

        public string FilePath { get; }
        public string FullName { get; }
        public string NameWithPublisher { get; }
        public VersionNumber Version { get; }

        public ALAppPackageFileInfo(string filePath)
        {
            this.FilePath = filePath;
            this.FullName = Path.GetFileNameWithoutExtension(this.FilePath);
            int versionPos = this.FullName.LastIndexOf("_");
            if (versionPos >= 0)
            {
                this.Version = new VersionNumber(this.FullName.Substring(versionPos + 1).Trim());
                this.NameWithPublisher = this.FullName.Substring(0, versionPos);
            }
            else
            {
                this.Version = new VersionNumber();
                this.NameWithPublisher = this.FullName;
            }
        }

    }
}
