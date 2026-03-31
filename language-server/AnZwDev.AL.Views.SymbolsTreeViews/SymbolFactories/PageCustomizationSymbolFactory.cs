using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class PageCustomizationSymbolFactory : PageCustomizationSymbolFactory<PageCustomizationSymbol>
    {
    }

    internal class PageCustomizationSymbolFactory<T> : ObjectExtensionSymbolFactory<T> where T : PageCustomizationSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.PageCustomizationObject;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.ControlChanges, ALSyntaxNodeKind.PageExtensionLayout, "layout", SymbolFactoryInstances.PageControlChangeSymbolFactory));

            base.CreateChildNodes(node, symbol);
        }

    }

}
