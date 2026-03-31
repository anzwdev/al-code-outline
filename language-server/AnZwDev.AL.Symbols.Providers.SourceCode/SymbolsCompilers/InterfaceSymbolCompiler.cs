using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class InterfaceSymbolCompiler
    {

        public static InterfaceSymbol Compile(InterfaceSyntax syntax, string? namespaceName, HashSet<string>? usings, string sourceFileName)
        {
            var id = 0;
            var name = NameCompiler.Compile(syntax.Name);
            var properties = PropertySymbolCompiler.Compile(syntax.PropertyList);
            (var methods, _, _) = CodeMemberSymbolCompiler.Compile(syntax.Members);

            return new InterfaceSymbol(id, new FullyQualifiedName(namespaceName, name), properties)
            {
                ReferenceSourceFileName = sourceFileName,
                Usings = usings,
                ExtendedInterfaces = ObjectReferenceCompiler.Compile(ObjectKind.Interface, usings, syntax.ExtendsInterfaces),
                Methods = methods
            };
        }


    }
}
