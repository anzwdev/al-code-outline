using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.DataTypeCompilers;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class MethodSymbolCompiler
    {

        public static MethodSymbol Compile(MethodDeclarationSyntax syntax)
        {
            var accessModifier = syntax.AccessModifier.Text;

            return new MethodSymbol()
            {
                Id = 0,
                MemberKind = SourceCodeSymbolsCompiler.CompileMemberKind(syntax),
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                IsLocal = accessModifier.Equals("local", StringComparison.OrdinalIgnoreCase),
                IsInternal = accessModifier.Equals("internal", StringComparison.OrdinalIgnoreCase),
                IsProtected = accessModifier.Equals("protected", StringComparison.OrdinalIgnoreCase),
                Attributes = AttributeSymbolCompiler.CompileList(syntax.Attributes),
                Parameters = MethodParameterSymbolCompiler.Compile(syntax.ParameterList),
                ReturnParameterDefinition = ReturnParameterDefinitionSymbolSymbolCompiler.Compile(syntax.ReturnValue)
            };
        }

    }
}
