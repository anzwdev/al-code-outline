using AnZwDev.AL.Symbols.Parsing;
using Microsoft.Dynamics.Nav.CodeAnalysis.SymbolReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppPermissionSetExtensionSymbol : AppObjectWithIdSymbol<PermissionSetExtensionSymbol>
    {

        [JsonPropertyName("TargetObject")]
        public string? TargetObject { get; set; }

        [JsonPropertyName("Permissions")]
        public AppPermissionSymbol[]? Permissions { get; set; }

        public override PermissionSetExtensionSymbol CreateSymbol(string? ns)
        {
            var properties = AppPropertySymbol.CreatePropertySymbolsCollection(Properties);
            HashSet<string>? usings = null;

            return new PermissionSetExtensionSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), properties)
            {
                ReferenceSourceFileName = ReferenceSourceFileName,
                Permissions = Permissions.CreateSymbolsListOrNull<PermissionSymbol, AppPermissionSymbol>(ns),
                Usings = usings,
                ExtendedObjectReference =  ALSymbolExpressionParser.ParseObjectReference(ObjectKind.PermissionSet, TargetObject, null),

                IncludedPermissionSets = ALSymbolExpressionParser.ParseObjectReferenceSeparatedListOrNull(
                    ObjectKind.PermissionSet, properties.IncludedPermissionSets, usings, ','),

                ExcludedPermissionSets = ALSymbolExpressionParser.ParseObjectReferenceSeparatedListOrNull(
                    ObjectKind.PermissionSet, properties.ExcludedPermissionSets, usings, ',')

            };
        }

    }
}
