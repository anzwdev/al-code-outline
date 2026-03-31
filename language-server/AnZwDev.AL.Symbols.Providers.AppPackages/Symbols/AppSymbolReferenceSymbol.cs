using AnZwDev.AL.Symbols.Collections;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppSymbolReferenceSymbol : AppObjectsContainerSymbol
    {

        [JsonPropertyName("AppId")]
        public string? AppId { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Publisher")]
        public string? Publisher { get; set; }

        [JsonPropertyName("Version")]
        public string? Version { get; set; }

        [JsonPropertyName("RuntimeVersion")]
        public string? RuntimeVersion { get; set; }


        [JsonPropertyName("InternalsVisibleToModules")]
        public AppInternalsVisibleToModuleSymbol[]? InternalsVisibleToModules { get; set; }

        public void UpdateSymbol(ApplicationSymbol symbol)
        {
            //Loaded from manifest
            //ProcessInternalsVisibleToModules(symbol);
            ProcessCollections(symbol, null);
        }

        /*
        private void ProcessInternalsVisibleToModules(ApplicationSymbol applicationSymbol)
        {
            if (InternalsVisibleToModules != null)
                for (int i = 0; i < InternalsVisibleToModules.Length; i++)
                {
                    var appSymbol = InternalsVisibleToModules[i];
                    if ((!String.IsNullOrWhiteSpace(appSymbol.AppId)) && (!applicationSymbol.Metadata.InternalsVisibleToModules.ContainsKey(appSymbol.AppId)))
                        applicationSymbol.Metadata  .InternalsVisibleToModules.Add(appSymbol.AppId, appSymbol.CreateSymbol());
                }
        }
        */

        protected override string? GetNamespace(string? parentNamespace)
        {
            return null;
        }

    }
}
