using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    public class AppPackagesSymbolReferencesCacheEntry
    {

        public AppPackageInformation PackageInformation { get; private set; }
        public ALAppSymbolReference Symbols { get; private set; }

        public AppPackagesSymbolReferencesCacheEntry(string fullPath) : this(new AppPackageInformation(fullPath))
        {
        }

        public AppPackagesSymbolReferencesCacheEntry(AppPackageInformation packageInformation)
        {
            this.PackageInformation = packageInformation;
            this.Symbols = AppPackageSymbolReferencesReader.Deserialize(packageInformation.FullPath);
        }

        public AppPackagesSymbolReferencesCacheEntry(AppPackageInformation packageInformation, AppPackagesSymbolReferencesCacheEntry sourceEntry)
        {
            this.PackageInformation = packageInformation;
            this.Symbols = sourceEntry.Symbols;
        }

        public void Reload(bool force)
        {
            //check if file changed
            if (!force)
                force = this.PackageInformation.FileUIdChanged();

            if (force)
            {
                this.PackageInformation.Reload();
                this.Symbols = AppPackageSymbolReferencesReader.Deserialize(this.PackageInformation.FullPath);
            }
        }

    }
}
