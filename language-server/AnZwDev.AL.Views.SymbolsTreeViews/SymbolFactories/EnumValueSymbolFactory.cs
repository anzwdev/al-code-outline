using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class EnumValueSymbolFactory : EnumValueSymbolFactory<EnumValueSymbol>
    {
    }

    internal class EnumValueSymbolFactory<T> : NamedSymbolWithPropertiesFactory<T> where T : EnumValueSymbol
    {

        override protected ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.EnumValue;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            node.Id = symbol.Ordinal;

            return node;
        }

    }
}
