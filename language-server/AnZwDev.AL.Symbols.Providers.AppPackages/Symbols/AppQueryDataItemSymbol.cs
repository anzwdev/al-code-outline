using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppQueryDataItemSymbol : AppSerializedSymbol<QueryDataItemSymbol>
    {

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Properties")]
        public AppPropertySymbol[]? Properties { get; set; }

        [JsonPropertyName("RelatedTable")]
        public string? RelatedTable { get; set; }

        [JsonPropertyName("DataItems")]
        public AppQueryDataItemSymbol[]? DataItems { get; set; }

        [JsonPropertyName("Columns")]
        public AppQueryColumnSymbol[]? Columns { get; set; }

        [JsonPropertyName("Filters")]
        public AppQueryColumnSymbol[]? Filters { get; set; }

        public override QueryDataItemSymbol CreateSymbol(string? ns)
        {
            HashSet<string>? usings = (String.IsNullOrWhiteSpace(ns)) ? null : new HashSet<string>() { ns };

            return new QueryDataItemSymbol()
            {
                Id = Id,
                Name = Name ?? String.Empty,
                Properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties),
                RelatedTable = ALSymbolExpressionParser.ParseObjectReference(ObjectKind.Table, RelatedTable, usings),
                DataItems = DataItems.CreateSymbolsListOrNull<QueryDataItemSymbol, AppQueryDataItemSymbol>(ns),
                Columns = Columns.CreateSymbolsListOrNull<QueryColumnSymbol, AppQueryColumnSymbol>(ns),
                Filters = Filters.CreateSymbolsListOrNull<QueryColumnSymbol, AppQueryColumnSymbol>(ns)
            };
        }


    }
}
