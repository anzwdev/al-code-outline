using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Parsing;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    public static class PermissionSetExtensionSymbolCompiler
    {

        public static PermissionSetExtensionSymbol Compile(PermissionSetExtensionSyntax syntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            var id = SimpleTypesCompiler.Compile(syntax.ObjectId);
            var name = NameCompiler.Compile(syntax.Name);
            var properties = PropertySymbolCompiler.Compile(syntax.PropertyList);

            return new PermissionSetExtensionSymbol(id, new FullyQualifiedName(namespaceName, name), properties)
            {
                ReferenceSourceFileName = sourceFileName,
                Usings = usings,
                ExtendedObjectReference = ObjectReferenceCompiler.Compile(ObjectKind.PermissionSet, usings, syntax.BaseObject),
                Permissions = PermissionSymbolCompiler.Compile(syntax.PropertyList, usings),

                IncludedPermissionSets = ALSymbolExpressionParser.ParseObjectReferenceSeparatedListOrNull(
                    ObjectKind.PermissionSet, properties.IncludedPermissionSets, usings, ','),

                ExcludedPermissionSets = ALSymbolExpressionParser.ParseObjectReferenceSeparatedListOrNull(
                    ObjectKind.PermissionSet, properties.ExcludedPermissionSets, usings, ',')
            };
        }

    }
}
