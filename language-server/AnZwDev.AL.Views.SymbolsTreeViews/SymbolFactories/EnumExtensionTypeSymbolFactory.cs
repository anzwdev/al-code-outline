using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class EnumExtensionTypeSymbolFactory : EnumExtensionTypeSymbolFactory<EnumExtensionTypeSymbol>
    {
    }

    internal class EnumExtensionTypeSymbolFactory<T> : ObjectExtensionSymbolFactory<T> where T : EnumExtensionTypeSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.EnumExtensionType;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            CollectionSymbolFactory.Append(node, symbol.Values, SymbolFactoryInstances.EnumValueSymbolFactory);
            base.CreateChildNodes(node, symbol);
        }

    }

}
