using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages
{
    public static class AppPackageSymbolsProvidersLoader
    {

        public static Dictionary<string, AppPackageSymbolsProvider> LoadFromFolder(string folderPath, AppPackageSymbolsCache? cache)
        {
            Dictionary<string, AppPackageSymbolsProvider> result = new Dictionary<string, AppPackageSymbolsProvider>(StringComparer.OrdinalIgnoreCase);

            var files = Directory.GetFiles(folderPath, "*.app", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; i++)
            {
                var provider = new AppPackageSymbolsProvider(files[i], cache);
                provider.Load(true);

                var symbols = provider.GetSymbols();
                if (symbols != null)
                {
                    AddOrUpdateAppSymbolProvider(result, provider, symbols, symbols.AppId);

                    if (SymbolsFacts.IsMicrosoftApp(symbols.AppId, symbols.Name, symbols.Publisher))
                    {
                        var altId = SymbolsFacts.GetMicrosoftAppAltId(symbols.AppId, symbols.Name, symbols.Publisher);
                        AddOrUpdateAppSymbolProvider(result, provider, symbols, altId);
                    }
                }
            }

            return result;
        }

        private static void AddOrUpdateAppSymbolProvider(Dictionary<string, AppPackageSymbolsProvider> list, AppPackageSymbolsProvider provider, ApplicationSymbol symbols, string? id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                if (list.ContainsKey(id))
                {
                    if ((list[id].GetSymbols()!.Version) < symbols.Version)
                        list[id] = provider;
                }
                else
                    list.Add(id, provider);
            }
        }


    }
}
