using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    public static class PermissionSymbolCompiler
    {

        public static List<PermissionSymbol>? Compile(PropertyListSyntax? syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            var valueSyntax = GetPermissionsValue(syntax.Properties);
            if (valueSyntax == null) 
                return null;

            return Compile(valueSyntax.PermissionProperties, usings);
        }

        public static List<PermissionSymbol>? Compile(PermissionPropertyValueSyntax? syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            return Compile(syntax.PermissionProperties, usings);
        }

        public static List<PermissionSymbol> Compile(SeparatedSyntaxList<PermissionSyntax> syntax, HashSet<string>? usings)
        {
            var list = new List<PermissionSymbol>(syntax.Count);
            for (int i = 0; i < syntax.Count; i++) 
            {
                var symbol = Compile(syntax[i], usings);
                if (symbol != null)
                    list.Add(symbol);
            }
            return list;
        }

        private static PermissionSymbol? Compile(PermissionSyntax? syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            var permissions = syntax.Permissions.Text;

            return new PermissionSymbol()
            {
                ObjectReference = ObjectReferenceCompiler.Compile(syntax.ObjectType, usings, syntax.ObjectReference),
                Value = new PermissionValue()
                {
                    Execute = GetPermissionLevel(permissions, 'x', 'X'),
                    Read = GetPermissionLevel(permissions, 'r', 'R'),
                    Insert = GetPermissionLevel(permissions, 'i', 'I'),
                    Modify = GetPermissionLevel(permissions, 'm', 'M'),
                    Delete = GetPermissionLevel(permissions, 'd', 'D')
                }
            };
        }

        private static PermissionLevel GetPermissionLevel(string value, char indirect, char direct)
        {
            if (value.Contains(indirect))
                return PermissionLevel.Indirect;
            if (value.Contains(direct))
                return PermissionLevel.Direct;
            return PermissionLevel.None;
        }

        private static PermissionPropertyValueSyntax? GetPermissionsValue(SyntaxList<PropertySyntaxOrEmpty> syntax)
        {
            for (int i=0; i<syntax.Count; i++)
            {
                var propOrEmpty = syntax[i];
                if ((propOrEmpty != null) && (propOrEmpty is PropertySyntax prop))
                {
                    var name = ALLiteralParser.ParseName(prop?.Name?.Identifier.Text);
                    if ((name != null) && (name.Equals("Permissions", StringComparison.OrdinalIgnoreCase)))
                        return prop!.Value as PermissionPropertyValueSyntax;
                }
            }
            return null;
        }

    }
}
