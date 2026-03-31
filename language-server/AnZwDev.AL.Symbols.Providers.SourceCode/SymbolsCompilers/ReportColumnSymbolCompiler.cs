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
    internal static class ReportColumnSymbolCompiler
    {

        public static ReportColumnSymbol? Compile(ReportColumnSyntax? syntax, string? owningDataItemName)
        {
            if (syntax == null)
                return null;

            return new ReportColumnSymbol()
            {
                Id = 0,
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                OwningDataItemName = owningDataItemName,
                SourceExpression = syntax.SourceExpression?.ToString(),
                TypeDefinition = null
            };
        }


    }
}
