using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class EnumTypeSymbolFactory : EnumTypeSymbolFactory<EnumTypeSymbol>
    {
    }

    internal class EnumTypeSymbolFactory<T> : ObjectSymbolFactory<T> where T : EnumTypeSymbol
    {
        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.EnumType;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            CollectionSymbolFactory.Append(node, symbol.Values, SymbolFactoryInstances.EnumValueSymbolFactory);
            base.CreateChildNodes(node, symbol);
        }
    }

}
