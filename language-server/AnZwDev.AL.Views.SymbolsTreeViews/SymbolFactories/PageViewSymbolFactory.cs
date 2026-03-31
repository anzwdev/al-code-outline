using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class PageViewSymbolFactory : PageViewSymbolFactory<PageViewSymbol>
    {
    }

    internal class PageViewSymbolFactory<T> : NamedSymbolWithIdAndPropertiesFactory<T> where T : PageViewSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.PageView;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            CollectionSymbolFactory.Append(node, symbol.ControlChanges, SymbolFactoryInstances.PageControlChangeSymbolFactory);

            base.CreateChildNodes(node, symbol);
        }

    }
}
