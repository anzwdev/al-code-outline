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
    internal static class EnumValueSymbolCompiler
    {

        public static List<EnumValueSymbol> Compile(SyntaxList<EnumValueSyntax> syntax)
        {
            var list = new List<EnumValueSymbol>();

            for (int i=0; i < syntax.Count; i++)
            {
            }

            return list;
        }

        public static EnumValueSymbol? Compile(EnumValueSyntax? syntax)
        {
            if (syntax == null)
                return null;

            return new EnumValueSymbol()
            { 
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Ordinal = SimpleTypesCompiler.CompileInt(syntax.Id),
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList)
            };
        }

    }
}
