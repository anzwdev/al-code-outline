using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppTableExtensionSymbol : AppObjectWithCodeSymbol<TableExtensionSymbol>
    {

        [JsonPropertyName("TargetObject")]
        public string? TargetObject { get; set; }

        [JsonPropertyName("Fields")]
        public AppTableFieldSymbol[]? Fields { get; set; }

        [JsonPropertyName("Keys")]
        public AppTableKeySymbol[]? Keys { get; set; }

        public override TableExtensionSymbol CreateSymbol(string? ns)
        {
            var properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties);

            return new TableExtensionSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), properties)
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                Variables = Variables.CreateSymbolsList<GlobalVariableDeclarationSymbol, AppGlobalVariableSymbol>(ns),
                Methods = Methods.CreateSymbolsList<MethodSymbol, AppMethodSymbol>(ns),
                Fields = Fields.CreateSymbolsList<TableFieldSymbol, AppTableFieldSymbol>(ns),
                Keys = Keys.CreateSymbolsList<TableKeySymbol, AppTableKeySymbol>(ns),
                FieldGroups = null,
                Usings = null,
                ExtendedObjectReference = ALSymbolExpressionParser.ParseObjectReference(ObjectKind.Table, TargetObject, null)
            };
        }


    }
}
