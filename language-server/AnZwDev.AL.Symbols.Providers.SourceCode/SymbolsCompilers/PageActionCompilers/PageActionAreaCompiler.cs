using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers;
using AnZwDev.System.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.PageActionCompilers
{
    internal static class PageActionAreaCompiler
    {

        public static PageActionSymbol? Compile(PageActionAreaSyntax? syntax)
        {
            if (syntax == null)
                return null;

            return new PageActionSymbol()
            {
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Kind = PageActionKind.Area,
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                Actions = PageActionSymbolCompiler.Compile(syntax.Actions),

                Id = 0,
                TargetId = 0,
                TargetName = null,
            };
        }

    }
}
