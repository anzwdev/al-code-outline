using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Symbols.Parsing;
using AnZwDev.AL.Symbols.Parsing.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppPropertySymbol
    {

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Value")]
        public string? Value { get; set; }

        public static PropertySymbolsCollection CreatePropertySymbolsCollection(AppPropertySymbol[]? appProperties)
        {
            var symbolsCollection = new PropertySymbolsCollection();
            if (appProperties != null)
                for (var i = 0; i < appProperties.Length; i++)
                    ALSymbolExpressionParser.ParsePropertyValue(symbolsCollection, appProperties[i].Name, appProperties[i].Value);
            return symbolsCollection;
        }

    }
}
