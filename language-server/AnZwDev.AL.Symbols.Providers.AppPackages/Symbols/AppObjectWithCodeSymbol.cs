using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal abstract class AppObjectWithCodeSymbol<T> : AppObjectWithIdSymbol<T> where T : ObjectSymbol
    {

        [JsonPropertyName("Variables")]
        public AppGlobalVariableSymbol[]? Variables { get; set; }

        [JsonPropertyName("Methods")]
        public AppMethodSymbol[]? Methods { get; set; }

    }
}
