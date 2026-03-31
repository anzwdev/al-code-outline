using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class PageActionSymbolFactory : PageActionSymbolFactory<PageActionSymbol>
    {
    }

    internal class PageActionSymbolFactory<T> : NamedSymbolWithIdAndPropertiesFactory<T> where T : PageActionSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return symbol.Kind.ToALSyntaxNodeKind();
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            CollectionSymbolFactory.Append(node, symbol.Actions, SymbolFactoryInstances.PageActionSymbolFactory);
            base.CreateChildNodes(node, symbol);
        }

    }
}
