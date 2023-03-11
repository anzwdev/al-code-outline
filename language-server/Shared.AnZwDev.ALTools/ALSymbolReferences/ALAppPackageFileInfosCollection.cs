using AnZwDev.ALTools.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPackageFileInfosCollection : List<ALAppPackageFileInfo>
    {

        public ALAppPackageFileInfosCollection()
        {
        }

        public void LoadFromFolder(string path)
        {
            this.Clear();

            string[] fileNames = Directory.GetFiles(path, "*.app");
            for (int i=0; i<fileNames.Length; i++)
            {
                this.Add(new ALAppPackageFileInfo(fileNames[i]));
            }
        }

        public ALAppPackageFileInfo Find(string publisher, string name, string version)
        {
            ALAppPackageFileInfo found = null;
            VersionNumber findVersion = new VersionNumber(version);
            string findNameWithPublisher = publisher + "_" + name;
            for (int i=0; i<this.Count; i++)
            {
                if ((this[i].NameWithPublisher.Equals(findNameWithPublisher, StringComparison.CurrentCultureIgnoreCase)) &&
                    (this[i].Version.GreaterOrEqual(findVersion)))
                {
                    found = this[i];
                    findVersion = found.Version;
                }
            }
            return found;
        }

    }
}
