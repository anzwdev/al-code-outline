using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class TableFieldGroupSymbolFactory : TableFieldGroupSymbolFactory<TableFieldGroupSymbol>
    {
    }

    internal class TableFieldGroupSymbolFactory<T> : NamedSymbolFactory<T> where T : TableFieldGroupSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.FieldGroup;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            if (symbol.FieldNames != null && symbol.FieldNames.Count > 0)
                node.FullName = ALLiteralFormatter.GetName(symbol.Name) + ": " + ALLiteralFormatter.GetNameList(symbol.FieldNames);

            return node;
        }

    }
}
