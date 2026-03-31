using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppAttributeArgumentSymbol
    {

        [JsonPropertyName("Value")]
        public string? Value { get; set; }

        public static List<string>? CreateSymbolCollection(AppAttributeArgumentSymbol[]? appSymbolsCollection)
        {
            if (appSymbolsCollection == null)
                return null;

            var list = new List<string>();
            for (int i = 0; i < appSymbolsCollection.Length; i++)
                if (appSymbolsCollection[i].Value != null)
                    list.Add(appSymbolsCollection[i].Value!);

            return list;
        }


    }
}
