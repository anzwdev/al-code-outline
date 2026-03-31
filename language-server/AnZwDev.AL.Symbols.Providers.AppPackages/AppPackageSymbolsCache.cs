using AnZwDev.System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages
{
    public class AppPackageSymbolsCache
    {

        private readonly Dictionary<string, AppPackageSymbolCacheEntry> _entries;

        public AppPackageSymbolsCache()
        {
            _entries = new Dictionary<string, AppPackageSymbolCacheEntry>(PathUtils.GetPathComparer());
        }

        public ApplicationSymbol? TryGet(string fullPath)
        {
            if ((_entries.ContainsKey(fullPath)) && (File.Exists(fullPath)))
            {
                var fileInfo = new FileInfo(fullPath);
                var entry = _entries[fullPath];

                if ((entry.LastWriteTimeUtc == fileInfo.LastWriteTimeUtc) && (entry.Length == fileInfo.Length))
                    return entry.Symbol;

                _entries.Remove(fullPath);
            }

            return null;
        }

        public void Add(ApplicationSymbol symbol)
        {
            if (symbol == null)
                return;
            
            var fullPath = symbol.ReferenceSourceFileName;
            if (File.Exists(fullPath))
            {
                var fileInfo = new FileInfo(fullPath);
                var entry = new AppPackageSymbolCacheEntry()
                {
                    Id = symbol.AppId ?? String.Empty,
                    FullPath = fullPath,
                    Length = fileInfo.Length,
                    LastWriteTimeUtc = fileInfo.LastWriteTimeUtc,
                    Symbol = symbol
                };

                if (_entries.ContainsKey(fullPath))
                    _entries[fullPath] = entry;
                else
                    _entries.Add(fullPath, entry);
            }
        }

        public void Clear()
        {
            _entries.Clear();
        }

    }
}
