using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.DataTypeCompilers;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.PageViewChangeCompilers
{
    internal static class PageViewMoveChangeCompiler
    {
        public static PageViewChangeSymbol? Compile(ViewMoveChangeSyntax syntax)
        {
            if (syntax == null)
                return null;

            return new PageViewChangeSymbol()
            {
                Anchor = NameCompiler.Compile(syntax.Anchor),
                ChangeKind = SourceCodeSymbolsCompiler.CompilePageViewChangeKind(syntax.ChangeKeyword),
                Views = PageViewSymbolCompiler.Compile(syntax.Views)
            };
        }
    }
}
