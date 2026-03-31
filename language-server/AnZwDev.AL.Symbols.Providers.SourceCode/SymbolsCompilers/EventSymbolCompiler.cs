using AnZwDev.AL.Symbols;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class EventSymbolCompiler
    {

        public static EventSymbol Compile(EventDeclarationSyntax syntax)
        {
            return new EventSymbol()
            {
                Id = 0,
                MemberKind = MemberKind.EventDeclaration,
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Attributes = AttributeSymbolCompiler.CompileList(syntax.Attributes),
                Parameters = MethodParameterSymbolCompiler.Compile(syntax.ParameterList),
                IsLocal = false,
                IsInternal = false,
                IsProtected = false,
                ReturnParameterDefinition = null
            };
        }


    }
}
