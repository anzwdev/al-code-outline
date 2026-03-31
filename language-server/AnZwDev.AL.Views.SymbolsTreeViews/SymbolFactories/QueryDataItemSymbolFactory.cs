using AnZwDev.AL.Symbols;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class QueryDataItemSymbolFactory : QueryDataItemSymbolFactory<QueryDataItemSymbol>
    {
    }

    internal class QueryDataItemSymbolFactory<T> : NamedSymbolWithIdAndPropertiesFactory<T> where T : QueryDataItemSymbol
    {
        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.QueryDataItem;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            if (symbol.RelatedTable != null)
                node.FullName = node.FullName + ": Table " + symbol.RelatedTable.Value.FullyQualifiedName;

            return node;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            CollectionSymbolFactory.Append(node, symbol.DataItems, SymbolFactoryInstances.QueryDataItemSymbolFactory);
            CollectionSymbolFactory.Append(node, symbol.Columns, SymbolFactoryInstances.QueryColumnSymbolFactory);
            CollectionSymbolFactory.Append(node, symbol.Filters, SymbolFactoryInstances.QueryColumnSymbolFactory);

            base.CreateChildNodes(node, symbol);
        }
    }
}
