using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class PageExtensionSymbolFactory : PageExtensionSymbolFactory<PageExtensionSymbol>
    {
    }

    internal class PageExtensionSymbolFactory<T> : ObjectExtensionWithCodeSymbolFactory<T> where T : PageExtensionSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.PageExtensionObject;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.ControlChanges, ALSyntaxNodeKind.PageExtensionLayout, "layout", SymbolFactoryInstances.PageControlChangeSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.ActionChanges, ALSyntaxNodeKind.PageExtensionActionList, "actions", SymbolFactoryInstances.PageActionChangeSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.ViewChanges, ALSyntaxNodeKind.PageExtensionViewList, "views", SymbolFactoryInstances.PageViewChangeSymbolFactory));

            base.CreateChildNodes(node, symbol);
        }

    }

}
