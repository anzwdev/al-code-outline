using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppTableSymbol : AppObjectWithCodeSymbol<TableSymbol>
    {

        [JsonPropertyName("Fields")]
        public AppTableFieldSymbol[]? Fields { get; set; }

        [JsonPropertyName("Keys")]
        public AppTableKeySymbol[]? Keys { get; set; }

        [JsonPropertyName("FieldGroups")]
        public AppTableFieldGroupSymbol[]? FieldGroups { get; set; }

        public override TableSymbol CreateSymbol(string? ns)
        {
            var symbol = new TableSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), AppPropertySymbol.CreatePropertySymbolsCollection(Properties))
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                Variables = Variables.CreateSymbolsList<GlobalVariableDeclarationSymbol, AppGlobalVariableSymbol>(ns),
                Methods = Methods.CreateSymbolsList<MethodSymbol, AppMethodSymbol>(ns),
                Fields = Fields.CreateSymbolsList<TableFieldSymbol, AppTableFieldSymbol>(ns),
                Keys = Keys.CreateSymbolsList<TableKeySymbol, AppTableKeySymbol>(ns),
                FieldGroups = FieldGroups.CreateSymbolsList<TableFieldGroupSymbol, AppTableFieldGroupSymbol>(ns),
                Usings = null
            };           

            return symbol;              
        }

    }
}
