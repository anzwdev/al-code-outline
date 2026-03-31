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
    public static class ObjectReferenceCompiler
    {

        public static ObjectReference Compile(SyntaxToken objectTypeToken, HashSet<string>? usings, ObjectNameOrIdSyntax? syntax)
        {
            var objectKind = EnumTypeCompilers.CompileObjectType(objectTypeToken);
            return Compile(objectKind, usings, syntax);
        }

        public static ObjectReference Compile(ObjectKind objectKind, HashSet<string>? usings, IdentifierNameSyntax? syntax)
        {
            return ALSymbolExpressionParser.ParseObjectReference(objectKind, syntax?.Identifier.Text, usings);
        }

        public static ObjectReference Compile(ObjectKind objectKind, HashSet<string>? usings, ObjectNameOrIdSyntax? syntax)
        {
            return ALSymbolExpressionParser.ParseObjectReference(objectKind, syntax?.Identifier?.ToString(), usings);
        }

        public static ObjectReference Compile(ObjectKind objectKind, HashSet<string>? usings, ObjectNameReferenceSyntax? syntax)
        {
            return ALSymbolExpressionParser.ParseObjectReference(objectKind, syntax?.Identifier?.ToString(), usings);
        }

        public static List<ObjectReference>? Compile(ObjectKind objectKind, HashSet<string>? usings, SeparatedSyntaxList<ObjectNameReferenceSyntax> syntax)
        {
            List<ObjectReference>? list = null;
            for (int i=0; i< syntax.Count; i++)
            {
                var objectReference = Compile(objectKind, usings, syntax[i]);
                if (!objectReference.IsEmpty())
                {
                    if (list == null)
                        list = new List<ObjectReference>();
                    list.Add(objectReference);
                }
            }
            return list;
        }

    }
}
