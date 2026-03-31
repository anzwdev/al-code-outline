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

    internal class ReportDataItemSymbolFactory : ReportDataItemSymbolFactory<ReportDataItemSymbol>
    {
    }

    internal class ReportDataItemSymbolFactory<T> : NamedSymbolWithIdAndPropertiesFactory<T> where T : ReportDataItemSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.ReportDataItem;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            if (symbol.RelatedTable != null)
                node.FullName = ALLiteralFormatter.GetName(symbol.Name) + ": " + symbol.RelatedTable.Value.FullyQualifiedName;

            return node;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            CollectionSymbolFactory.Append(node, symbol.Columns, SymbolFactoryInstances.ReportColumnSymbolFactory);
            CollectionSymbolFactory.Append(node, symbol.DataItems, SymbolFactoryInstances.ReportDataItemSymbolFactory);

            base.CreateChildNodes(node, symbol);
        }

    }
}
