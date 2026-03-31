using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.AppPackages.Metadata;
using AnZwDev.AL.Symbols.Providers.AppPackages.Symbols;
using AnZwDev.System.IO;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Packaging;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace AnZwDev.AL.Symbols.Providers.AppPackages
{
    public class AppPackageSymbolsProvider : SymbolsProvider
    {

        public string FilePath { get; }
        public AppPackageSymbolsCache? Cache { get; }

        public AppPackageSymbolsProvider(string path, AppPackageSymbolsCache? cache)
        {
            FilePath = path;
            Cache = cache;
        }

        public override void Load(bool metadataOnly)
        {
            var appSymbol = Cache?.TryGet(FilePath);
            
            if (appSymbol == null)
            {
                using (var stream = FileHelper.OpenFileStreamWithRetry(FilePath))
                {
                    if (stream != null)
                    {
                        using (var navAppPackage = NavAppPackage.Open(stream, false))
                        using (var naAppPackageReader = new NavAppPackageReader(stream, navAppPackage, false))
                        {
                            var navAppManifest = naAppPackageReader.ReadNavAppManifest();
                            if (navAppManifest != null)
                            {

                                appSymbol = NavAppManifestLoader.CreateApplicationSymbol(navAppManifest, FilePath);

                                if (!metadataOnly)
                                {
                                    using (var symbolsStream = naAppPackageReader.ReadSymbolReferenceFile())
                                    {
                                        var deserialized = JsonSerializer.Deserialize<AppSymbolReferenceSymbol>(symbolsStream);
                                        deserialized?.UpdateSymbol(appSymbol);
                                    }

                                    Cache?.Add(appSymbol);
                                }
                            }
                        }
                    }
                }
            }

            SetSymbols(appSymbol);
        }

    }
}
