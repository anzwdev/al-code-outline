using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class EnumExtensionTypeSymbolCompiler
    {

        public static EnumExtensionTypeSymbol Compile(EnumExtensionTypeSyntax syntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            var id = SimpleTypesCompiler.Compile(syntax.ObjectId);
            var name = NameCompiler.Compile(syntax.Name);
            var properties = PropertySymbolCompiler.Compile(syntax.PropertyList);

            return new EnumExtensionTypeSymbol(id, new FullyQualifiedName(namespaceName, name), properties)
            {
                ReferenceSourceFileName = sourceFileName,
                Usings = usings,
                Values = EnumValueSymbolCompiler.Compile(syntax.Values),
                ExtendedObjectReference = ObjectReferenceCompiler.Compile(ObjectKind.EnumType, usings, syntax.BaseObject)
            };
        }


    }
}
