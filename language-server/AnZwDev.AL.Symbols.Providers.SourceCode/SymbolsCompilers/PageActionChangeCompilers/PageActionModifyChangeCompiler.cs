using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.PageActionChangeCompilers
{
    internal static class PageActionModifyChangeCompiler
    {

        public static PageActionChangeSymbol? Compile(ActionModifyChangeSyntax syntax)
        {
            if (syntax == null)
                return null;

            return new PageActionChangeSymbol()
            {
                Anchor = NameCompiler.Compile(syntax.Name),
                ChangeKind = PageActionChangeKind.Modify,
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                Actions = null
            };
        }


    }
}
