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

    internal class PageSymbolFactory : PageSymbolFactory<PageSymbol>
    {
    }

    internal class PageSymbolFactory<T> : ObjectWithCodeSymbolFactory<T> where T : PageSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.PageObject;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Controls, ALSyntaxNodeKind.PageLayout, "layout", SymbolFactoryInstances.PageControlSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Actions, ALSyntaxNodeKind.PageActionList, "actions", SymbolFactoryInstances.PageActionSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Views, ALSyntaxNodeKind.PageViewList, "views", SymbolFactoryInstances.PageViewSymbolFactory));

            base.CreateChildNodes(node, symbol);
        }

    }

}
