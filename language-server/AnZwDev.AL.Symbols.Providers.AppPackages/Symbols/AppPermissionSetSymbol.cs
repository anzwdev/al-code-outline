using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppPermissionSetSymbol : AppObjectWithIdSymbol<PermissionSetSymbol>
    {

        [JsonPropertyName("Permissions")]
        public AppPermissionSymbol[]? Permissions { get; set; }

        public override PermissionSetSymbol CreateSymbol(string? ns)
        {
            var properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties);
            HashSet<string>? usings = null;

            return new PermissionSetSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), properties)
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                Permissions = Permissions.CreateSymbolsListOrNull<PermissionSymbol, AppPermissionSymbol>(ns),
                Usings = usings,

                IncludedPermissionSets = ALSymbolExpressionParser.ParseObjectReferenceSeparatedListOrNull(
                    ObjectKind.PermissionSet, properties.IncludedPermissionSets, usings, ','),

                ExcludedPermissionSets = ALSymbolExpressionParser.ParseObjectReferenceSeparatedListOrNull(
                    ObjectKind.PermissionSet, properties.ExcludedPermissionSets, usings, ',')

            };
        }


    }
}
