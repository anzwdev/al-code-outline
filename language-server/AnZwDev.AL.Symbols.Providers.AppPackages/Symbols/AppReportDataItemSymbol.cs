using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Parsing;
using AnZwDev.AL.Symbols.Parsing.Parsers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppReportDataItemSymbol : AppSerializedSymbol<ReportDataItemSymbol>
    {

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Properties")]
        public AppPropertySymbol[]? Properties { get; set; }

        [JsonPropertyName("OwningDataItemName")]
        public string? OwningDataItemName { get; set; }

        [JsonPropertyName("RelatedTable")]
        public string? RelatedTable { get; set; }

        [JsonPropertyName("Indentation")]
        public int Indentation { get; set; }

        [JsonPropertyName("FilterControlId")]
        public int FilterControlId { get; set; }

        [JsonPropertyName("Columns")]
        public AppReportColumnSymbol[]? Columns { get; set; }

        [JsonPropertyName("DataItems")]
        public AppReportDataItemSymbol[]? DataItems { get; set; }


        public override ReportDataItemSymbol CreateSymbol(string? ns)
        {
            HashSet<string>? usings = (String.IsNullOrWhiteSpace(ns)) ? null : new HashSet<string>() { ns };

            return new ReportDataItemSymbol()
            {
                Id = Id,
                Name = Name ?? String.Empty,
                Properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties),
                OwningDataItemName = OwningDataItemName,
                RelatedTable = ALSymbolExpressionParser.ParseObjectReference(ObjectKind.Table, RelatedTable, usings),
                Indentation = Indentation,
                FilterControlId = FilterControlId,
                Columns = Columns.CreateSymbolsListOrNull<ReportColumnSymbol, AppReportColumnSymbol>(ns),
                DataItems = DataItems.CreateSymbolsListOrNull<ReportDataItemSymbol, AppReportDataItemSymbol>(ns)
            };
        }


    }
}
