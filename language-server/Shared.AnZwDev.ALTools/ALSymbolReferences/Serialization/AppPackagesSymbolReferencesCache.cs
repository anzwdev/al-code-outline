using AnZwDev.ALTools.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{
    public class AppPackagesSymbolReferencesCache
    {

        protected Dictionary<string, AppPackagesSymbolReferencesCacheEntry> _packages;

        public AppPackagesSymbolReferencesCache()
        {
            _packages = new Dictionary<string, AppPackagesSymbolReferencesCacheEntry>();
        }

        protected AppPackagesSymbolReferencesCacheEntry FindOrLoadCacheEntry(string filePath)
        {
            //find entry for the file path
            if (_packages.ContainsKey(filePath))
            {
                AppPackagesSymbolReferencesCacheEntry entry = _packages[filePath];
                entry.Reload(false);
                return entry;
            }

            //find entry for the same file but from different path
            AppPackageInformation packageInformation = new AppPackageInformation(filePath);
            if (packageInformation.IsEmpty())
                return null;

            foreach (AppPackagesSymbolReferencesCacheEntry cacheEntry in _packages.Values)
            {
                if (cacheEntry.PackageInformation.TheSameContent(packageInformation))
                {
                    AppPackagesSymbolReferencesCacheEntry newCacheEntry = new AppPackagesSymbolReferencesCacheEntry(packageInformation, cacheEntry);
                    _packages.Add(filePath, newCacheEntry);
                    return newCacheEntry;
                }
            }

            //load new entry
            AppPackagesSymbolReferencesCacheEntry newEntry = new AppPackagesSymbolReferencesCacheEntry(packageInformation);
            _packages.Add(filePath, newEntry);
            return newEntry;
        }

        public ALAppSymbolReference GetSymbolReference(string filePath)
        {
            AppPackagesSymbolReferencesCacheEntry cacheEntry = this.FindOrLoadCacheEntry(filePath);
            if (cacheEntry != null)
                return cacheEntry.Symbols;
            return null;
        }

        public void RemoveDeletedFiles()
        {
            List<string> deleted = null;
            foreach (string filePath in _packages.Keys)
            {
                if (!File.Exists(filePath))
                {
                    if (deleted == null)
                        deleted = new List<string>();
                    deleted.Add(filePath);
                }
            }

            this.Remove(deleted);
        }

        public void RemoveFilesFromPath(string parentPath)
        {
            List<string> list = null;
            foreach (string path in _packages.Keys)
            {
                if (PathUtils.ContainsPath(parentPath, path))
                {
                    if (list == null)
                        list = new List<string>();
                    list.Add(path);
                }
            }
            this.Remove(list);
        }

        public void Remove(List<string> pathsList)
        {
            if (pathsList != null)
            {
                for (int i=0; i<pathsList.Count; i++)
                {
                    _packages.Remove(pathsList[i]);
                }
            }
        }

        public bool Remove(string path)
        {
            if (_packages.ContainsKey(path))
            {
                _packages.Remove(path);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            _packages.Clear();
        }

    }
}
