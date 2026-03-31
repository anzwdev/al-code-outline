using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppNamespaceSymbol : AppObjectsContainerSymbol
    {

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        protected override string? GetNamespace(string? parentNamespace)
        {
            var name = Name ?? String.Empty;

            if (String.IsNullOrEmpty(parentNamespace))
                return name;

            return parentNamespace + "." + name;
        }

    }
}
