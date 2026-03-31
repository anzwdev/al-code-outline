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
    internal static class PageViewSymbolCompiler
    {

        public static List<PageViewSymbol>? Compile(PageViewListSyntax? syntax, HashSet<string>? usings)
        {
            if (syntax == null)
                return null;
            return Compile(syntax.Views, usings);
        }
        public static List<PageViewSymbol>? Compile(SyntaxList<PageViewSyntax> viewsList, HashSet<string>? usings)
        {
            if (viewsList.Count == 0)
                return null;

            List<PageViewSymbol> list = new List<PageViewSymbol>();
            for (int i = 0; i < viewsList.Count; i++)
            {
                var view = Compile(viewsList[i], usings);
                if (view != null)
                    list.Add(view);
            }

            return list;
        }

        private static PageViewSymbol? Compile(PageViewSyntax syntax, HashSet<string>? usings)
        {
            return new PageViewSymbol()
            {
                Id = 0,
                Name = NameCompiler.Compile(syntax.Name).NotNull(),
                Properties = PropertySymbolCompiler.Compile(syntax.PropertyList),
                ControlChanges = PageControlChangesSymbolCompiler.Compile(syntax.Layout, usings)
            };
        }

        public static List<PageViewSymbol> Compile(SeparatedSyntaxList<IdentifierNameSyntax> syntax)
        {
            if (syntax.Count == 0)
                return new List<PageViewSymbol>();

            var list = new List<PageViewSymbol>(syntax.Count);
            for (int i = 0; i < syntax.Count; i++)
            {
                var item = Compile(syntax[i]);
                if (item != null)
                    list.Add(item);
            }
            return list;
        }


        private static PageViewSymbol? Compile(IdentifierNameSyntax? syntax)
        {
            var name = NameCompiler.Compile(syntax);
            if (name == null)
                return null;

            return new PageViewSymbol()
            {
                Name = name,
                Properties = null,
                ControlChanges = null,
                Id = 0
            };
        }

    }
}
