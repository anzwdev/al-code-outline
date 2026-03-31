using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class QueryColumnSymbolFactory : QueryColumnSymbolFactory<QueryColumnSymbol>
    {
    }

    internal class QueryColumnSymbolFactory<T> : NamedSymbolWithIdAndPropertiesFactory<T> where T : QueryColumnSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.QueryColumn;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            if (!String.IsNullOrWhiteSpace(symbol.SourceColumn))
                node.FullName = ALLiteralFormatter.GetName(symbol.Name) + ": " + ALLiteralFormatter.GetName(symbol.SourceColumn);

            return node;
        }

    }
}
