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
    internal static class PageActionSeparatorCompiler
    {

        public static PageActionSymbol? Compile(PageActionSeparatorSyntax syntax)
        {
            if (syntax == null)
                return null;

            return new PageActionSymbol()
            {
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Kind = PageActionKind.Separator,
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                Actions = null,

                Id = 0,
                TargetId = 0,
                TargetName = null,
            };
        }


    }
}
