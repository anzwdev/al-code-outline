using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal abstract class AppObjectWithIdSymbol<T> : AppObjectSymbol<T> where T : ObjectSymbol
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

    }
}
