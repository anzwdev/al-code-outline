using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal class ReportColumnSymbolFactory : ReportColumnSymbolFactory<ReportColumnSymbol>
    {
    }

    internal class ReportColumnSymbolFactory<T> : NamedSymbolWithIdAndPropertiesFactory<T> where T : ReportColumnSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.ReportColumn;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            if (!string.IsNullOrWhiteSpace(symbol.SourceExpression))
                node.FullName = ALLiteralFormatter.GetName(node.Name) + ": " + symbol.SourceExpression;

            return node;
        }

    }
}
