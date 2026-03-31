using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppReportLabelSymbol : AppSerializedSymbol<ReportLabelSymbol>
    {

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }


        public override ReportLabelSymbol CreateSymbol(string? ns)
        {
            return new ReportLabelSymbol()
            {
                Id = Id,
                Name = Name ?? String.Empty
            };
        }


    }
}
