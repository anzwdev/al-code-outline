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
    internal static class DotNetAssemblyDeclarationsCompiler
    {

        public static List<DotNetAssemblyDeclarationSymbol> Compile(SyntaxList<DotNetAssemblySyntax> syntax, string? sourceFileName)
        {
            var list = new List<DotNetAssemblyDeclarationSymbol>(syntax.Count);
            for (int i = 0; i < syntax.Count; i++)
            {
                var symbol = Compile(syntax[i], sourceFileName);
                if (symbol != null)
                    list.Add(symbol);
            }
            return list;
        }
            
        public static DotNetAssemblyDeclarationSymbol? Compile(DotNetAssemblySyntax? syntax, string? sourceFileName)
        {
            if (syntax == null)
                return null;

            return new DotNetAssemblyDeclarationSymbol()
            {
                Name = NameCompiler.Compile(syntax.AssemblyName).NotNull(),
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                TypeDeclarations = DotNetTypeSymbolCompiler.Compile(syntax.TypeDeclarations, sourceFileName)        
            };
        }

    }
}
