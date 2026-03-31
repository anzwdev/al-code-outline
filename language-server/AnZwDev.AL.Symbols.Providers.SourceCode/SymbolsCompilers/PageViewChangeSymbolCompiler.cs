using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers.PageViewChangeCompilers;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class PageViewChangeSymbolCompiler
    {

        public static List<PageViewChangeSymbol>? Compile(PageExtensionViewListSyntax? syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;

            return Compile(syntax.Changes, usings);
        }

        public static List<PageViewChangeSymbol>? Compile<T>(SyntaxList<T> controlsList, HashSet<string>? usings) where T : ViewChangeBaseSyntax
        {
            if (controlsList.Count == 0)
                return null;

            List<PageViewChangeSymbol> list = new List<PageViewChangeSymbol>();
            for (int i = 0; i < controlsList.Count; i++)
            {
                var item = Compile(controlsList[i], usings);
                if (item != null)
                    list.Add(item);
            }

            return list;
        }

        private static PageViewChangeSymbol? Compile(ViewChangeBaseSyntax syntax, HashSet<string>? usings)
        {
            switch (syntax)
            {
                case ViewAddChangeSyntax addChangeSyntax:
                    return PageViewAddChangeCompiler.Compile(addChangeSyntax, usings);
                case ViewModifyChangeSyntax modifyChangeSyntax:
                    return PageViewModifyChangeCompiler.Compile(modifyChangeSyntax);
                case ViewMoveChangeSyntax moveChangeSyntax:
                    return PageViewMoveChangeCompiler.Compile(moveChangeSyntax);
            }

            return null;
        }



    }
}
