using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppReportColumnSymbol : AppSerializedSymbol<ReportColumnSymbol>
    {

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Properties")]
        public AppPropertySymbol[]? Properties { get; set; }

        [JsonPropertyName("OwningDataItemName")]
        public string? OwningDataItemName { get; set; }

        [JsonPropertyName("TypeDefinition")]
        public AppTypeDefinitionSymbol? TypeDefinition { get; set; }


        public override ReportColumnSymbol CreateSymbol(string? ns)
        {
            var propertiesSymbol = AppPropertySymbol.CreatePropertySymbolsCollection(Properties);
            var sourceExpression = propertiesSymbol.SourceExpression;

            return new ReportColumnSymbol()
            {
                Id = Id,
                Name = Name ?? String.Empty,
                Properties = propertiesSymbol,
                OwningDataItemName = OwningDataItemName,
                TypeDefinition = TypeDefinition?.CreateSymbol(ns),
                SourceExpression = sourceExpression
            };
        }

    }
}
