using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class DotNetTypeSymbolCompiler
    {

        public static List<DotNetTypeDeclarationSymbol> Compile(SyntaxList<DotNetTypeDeclarationSyntax> syntax, string? sourceFileName)
        {
            var list = new List<DotNetTypeDeclarationSymbol>(syntax.Count);
            for (int i = 0; i < syntax.Count; i++)
            {
                var symbol = Compile(syntax[i], sourceFileName);
                if (symbol != null)
                    list.Add(symbol);
            }

            return list;
        }

        public static DotNetTypeDeclarationSymbol? Compile(DotNetTypeDeclarationSyntax? syntax, string? sourceFileName)
        {
            if (syntax == null)
                return null;

            return new DotNetTypeDeclarationSymbol()
            {
                TypeName = NameCompiler.Compile(syntax.TypeName),
                AliasName = NameCompiler.Compile(syntax.TypeAlias),
                ReferenceSourceFileName = sourceFileName
            };
        }

    }
}
