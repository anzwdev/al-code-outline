using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages
{
    internal class AppPackageSymbolCacheEntry
    {

        public required string Id { get; init; }
        public required string FullPath { get; init; }
        public required long Length { get; init; }
        public required DateTime LastWriteTimeUtc { get; init; }
        public required ApplicationSymbol Symbol { get; init; }

    }
}
