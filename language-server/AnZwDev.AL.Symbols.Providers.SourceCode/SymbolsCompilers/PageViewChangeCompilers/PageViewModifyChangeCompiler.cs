using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.PageViewChangeCompilers
{
    internal static class PageViewModifyChangeCompiler
    {
        public static PageViewChangeSymbol? Compile(ViewModifyChangeSyntax syntax)
        {
            if (syntax == null)
                return null;

            return new PageViewChangeSymbol()
            {
                Anchor = NameCompiler.Compile(syntax.Name),
                ChangeKind = PageViewChangeKind.Modify,                
                Views = null
            };
        }
    }
}
