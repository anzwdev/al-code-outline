using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppReportExtensionSymbol : AppObjectWithCodeSymbol<ReportExtensionSymbol>
    {

        [JsonPropertyName("Target")]
        public string? Target { get; set; }

        [JsonPropertyName("RequestPage")]
        public AppRequestPageExtensionSymbol? RequestPage { get; set; }

        [JsonPropertyName("DataItems")]
        public AppReportDataItemSymbol[]? DataItems { get; set; }

        [JsonPropertyName("Columns")]
        public AppReportColumnSymbol[]? Columns { get; set; }

        [JsonPropertyName("Labels")]
        public AppReportLabelSymbol[]? Labels { get; set; }

        [JsonPropertyName("Layouts")]
        public AppReportLayoutSymbol[]? Layouts { get; set; }

        public override ReportExtensionSymbol CreateSymbol(string? ns)
        {
            return new ReportExtensionSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), AppPropertySymbol.CreatePropertySymbolsCollection(Properties))
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                RequestPage = RequestPage?.CreateSymbol(ns),
                DataItems = DataItems.CreateSymbolsListOrNull<ReportDataItemSymbol, AppReportDataItemSymbol>(ns),
                Columns = Columns.CreateSymbolsListOrNull<ReportColumnSymbol, AppReportColumnSymbol>(ns),
                Labels = Labels.CreateSymbolsListOrNull<ReportLabelSymbol, AppReportLabelSymbol>(ns),
                Layouts = Layouts.CreateSymbolsListOrNull<ReportLayoutSymbol, AppReportLayoutSymbol>(ns),
                Variables = Variables.CreateSymbolsList<GlobalVariableDeclarationSymbol, AppGlobalVariableSymbol>(ns),
                Methods = Methods.CreateSymbolsList<MethodSymbol, AppMethodSymbol>(ns),
                Usings = null,
                ExtendedObjectReference = ALSymbolExpressionParser.ParseObjectReference(ObjectKind.Report, Target, null)
            };
        }

    }
}
