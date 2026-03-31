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
    internal static class QueryColumnSymbolCompiler
    {

        public static QueryColumnSymbol? Compile(QueryElementSyntax? syntax)
        {
            if (syntax == null)
                return null;

            return new QueryColumnSymbol()
            {
                Id = 0,
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                SourceColumn = NameCompiler.Compile(syntax.RelatedField)
            };
        }

    }
}
