using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.PageControlChangeCompilers
{
    internal static class PageControlModifyChangeCompiler
    {

        public static PageControlChangeSymbol? Compile(ControlModifyChangeSyntax syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            return new PageControlChangeSymbol()
            {
                Anchor = NameCompiler.Compile(syntax.Name),
                ChangeKind = PageControlChangeKind.Modify,
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                Controls = null
            };
        }


    }
}
