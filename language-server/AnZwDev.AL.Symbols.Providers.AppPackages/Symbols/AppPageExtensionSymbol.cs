using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppPageExtensionSymbol : AppObjectWithCodeSymbol<PageExtensionSymbol>
    {

        [JsonPropertyName("TargetObject")]
        public string? TargetObject { get; set; }

        [JsonPropertyName("ControlChanges")]
        public AppPageControlChangeSymbol[]? ControlChanges { get; set; }

        [JsonPropertyName("ActionChanges")]
        public AppPageActionChangeSymbol[]? ActionChanges { get; set; }

        [JsonPropertyName("ViewChanges")]
        public AppPageViewChangeSymbol[]? ViewChanges { get; set; }


        public override PageExtensionSymbol CreateSymbol(string? ns)
        {
            var properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties);

            return new PageExtensionSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), properties)
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                Variables = Variables.CreateSymbolsList<GlobalVariableDeclarationSymbol, AppGlobalVariableSymbol>(ns),
                Methods = Methods.CreateSymbolsList<MethodSymbol, AppMethodSymbol>(ns),
                ControlChanges = ControlChanges.CreateSymbolsListOrNull<PageControlChangeSymbol, AppPageControlChangeSymbol>(ns),
                ActionChanges = ActionChanges.CreateSymbolsListOrNull<PageActionChangeSymbol, AppPageActionChangeSymbol>(ns),
                ViewChanges = ViewChanges.CreateSymbolsListOrNull<PageViewChangeSymbol, AppPageViewChangeSymbol>(ns),
                Usings = null,
                ExtendedObjectReference = ALSymbolExpressionParser.ParseObjectReference(ObjectKind.Page, TargetObject, null)
            };
        }

    }
}
