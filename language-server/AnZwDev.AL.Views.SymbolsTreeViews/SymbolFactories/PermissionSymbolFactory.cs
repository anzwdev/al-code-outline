using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class PermissionSymbolFactory : PermissionSymbolFactory<PermissionSymbol>
    {
    }

    internal class PermissionSymbolFactory<T> : SymbolFactory<T> where T : PermissionSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.Permission;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            node.Name = node.Name + " (" + DisplayStringFormatter.FormatFullyQualifiedName(symbol.ObjectReference.FullyQualifiedName) + ")";

            return node;
        }

    }
}
