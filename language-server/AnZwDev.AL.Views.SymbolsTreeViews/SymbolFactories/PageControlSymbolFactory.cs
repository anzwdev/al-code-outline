using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal class PageControlSymbolFactory : PageControlSymbolFactory<PageControlSymbol>
    {
    }

    internal class PageControlSymbolFactory<T> : NamedSymbolWithIdAndPropertiesFactory<T> where T : PageControlSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return symbol.Kind.ToALSyntaxNodeKind();
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            CollectionSymbolFactory.Append(node, symbol.Controls, SymbolFactoryInstances.PageControlSymbolFactory);
            base.CreateChildNodes(node, symbol);
        }

    }
}
