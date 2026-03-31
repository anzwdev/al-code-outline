using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppReportSymbol : AppObjectWithCodeSymbol<ReportSymbol>
    {

        [JsonPropertyName("RequestPage")]
        public AppRequestPageSymbol? RequestPage { get; set; }

        [JsonPropertyName("DataItems")]
        public AppReportDataItemSymbol[]? DataItems { get; set; }

        [JsonPropertyName("Labels")]
        public AppReportLabelSymbol[]? Labels { get; set; }

        [JsonPropertyName("Layouts")]
        public AppReportLayoutSymbol[]? Layouts { get; set; }

        public override ReportSymbol CreateSymbol(string? ns)
        {
            return new ReportSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), AppPropertySymbol.CreatePropertySymbolsCollection(Properties))
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                Variables = Variables.CreateSymbolsList<GlobalVariableDeclarationSymbol, AppGlobalVariableSymbol>(ns),
                Methods = Methods.CreateSymbolsList<MethodSymbol, AppMethodSymbol>(ns),
                RequestPage = RequestPage?.CreateSymbol(ns),
                DataItems = DataItems.CreateSymbolsListOrNull<ReportDataItemSymbol, AppReportDataItemSymbol>(ns),
                Labels = Labels.CreateSymbolsListOrNull<ReportLabelSymbol, AppReportLabelSymbol>(ns),
                Layouts = Layouts.CreateSymbolsListOrNull<ReportLayoutSymbol, AppReportLayoutSymbol>(ns),
                Usings = null
            };
        }

    }

}
