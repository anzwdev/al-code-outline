using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppPageSymbol : AppObjectWithCodeSymbol<PageSymbol>
    {

        [JsonPropertyName("HasActionsV2")]
        public bool HasActionsV2 { get; set; }

        [JsonPropertyName("Controls")]
        public AppPageControlSymbol[]? Controls { get; set; }

        [JsonPropertyName("Actions")]
        public AppPageActionSymbol[]? Actions { get; set; }

        [JsonPropertyName("Views")]
        public AppPageViewSymbol[]? Views { get; set; }


        public override PageSymbol CreateSymbol(string? ns)
        {
            HashSet<string>? usings = null;
            var propertiesSymbol = AppPropertySymbol.CreatePropertySymbolsCollection(Properties);
            var sourceTableReference = ALSymbolExpressionParser.ParseObjectReference(ObjectKind.Table, propertiesSymbol.SourceTable, usings);

            return new PageSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), propertiesSymbol)
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                Variables = Variables.CreateSymbolsList<GlobalVariableDeclarationSymbol, AppGlobalVariableSymbol>(ns),
                Methods = Methods.CreateSymbolsList<MethodSymbol, AppMethodSymbol>(ns),
                HasActionsV2 = HasActionsV2,
                Controls = Controls.CreateSymbolsListOrNull<PageControlSymbol, AppPageControlSymbol>(ns),
                Actions = Actions.CreateSymbolsListOrNull<PageActionSymbol, AppPageActionSymbol>(ns),
                Views = Views.CreateSymbolsListOrNull<PageViewSymbol, AppPageViewSymbol>(ns),
                Usings = usings,
                SourceTable = sourceTableReference
            };
        }

    }
}
