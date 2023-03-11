using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.Extensions;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    public class AppPackageInformationsCollection : List<AppPackageInformation>
    {

        public AppPackageInformationsCollection()
        {
        }

        public void AddFromFolder(string path)
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, "*.app");
                for (int i = 0; i < files.Length; i++)
                {
                    AppPackageInformation packageInfo = new AppPackageInformation(files[i]);
                    if (!packageInfo.IsEmpty())
                        this.Add(packageInfo);
                }
            }
        }

        public AppPackageInformation Find(string id, string name, string publisher, VersionNumber version)
        {
            bool emptyId = String.IsNullOrWhiteSpace(id);
            AppPackageInformation foundPackage = null;

            name = name.NotNull();
            publisher = publisher.NotNull();

            for (int i = 0; i < this.Count; i++)
            {
                if (!this[i].IsEmpty())
                {
                    NavxApp app = this[i].Manifest.App;
                    bool compareId = ((!emptyId) && (!String.IsNullOrWhiteSpace(app.Id)));

                    if (
                        (
                            (compareId) &&
                            (id.Equals(app.Id, StringComparison.CurrentCultureIgnoreCase))
                        ) || (
                            (!compareId) &&
                            (name.Equals(app.Name, StringComparison.CurrentCultureIgnoreCase)) &&
                            (publisher.Equals(app.Publisher, StringComparison.CurrentCultureIgnoreCase))
                        ))
                    {
                        if ((foundPackage == null) || (app.Version.Greater(version)))
                        {
                            foundPackage = this[i];
                            version = this[i].Manifest.App.Version;
                        }
                    }
                }
            }

            return foundPackage;
        }

    }
}
