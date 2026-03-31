using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class TableFieldGroupExtensionSymbolFactory : TableFieldGroupExtensionSymbolFactory<TableFieldGroupExtensionSymbol>
    {
    }

    internal class TableFieldGroupExtensionSymbolFactory<T> : SymbolFactory<T> where T : TableFieldGroupExtensionSymbol
    {
        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.FieldGroupAddChange;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            var name = "Add";
            if (!string.IsNullOrEmpty(symbol.Anchor))
                name = name + " (" + symbol.Anchor + ")";

            node.Name = name;
            node.FullName = name;

            if (symbol.FieldNames != null && symbol.FieldNames.Count > 0)
                node.FullName = node.FullName + ": " + ALLiteralFormatter.GetNameList(symbol.FieldNames);

            return node;
        }


    }
}
