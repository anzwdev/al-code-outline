using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class PageViewChangeSymbolFactory : PageViewChangeSymbolFactory<PageViewChangeSymbol>
    {
    }

    internal class PageViewChangeSymbolFactory<T> : SymbolFactory<T> where T : PageViewChangeSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return symbol.ChangeKind.ToALSyntaxNodeKind();
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            var name = symbol.ChangeKind.ToString();
            if (!string.IsNullOrEmpty(symbol.Anchor))
                name = name + " (" + symbol.Anchor + ")";

            node.Name = name;
            node.FullName = name;

            return node;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            CollectionSymbolFactory.Append(node, symbol.Views, SymbolFactoryInstances.PageViewSymbolFactory);

            base.CreateChildNodes(node, symbol);
        }

    }
}
