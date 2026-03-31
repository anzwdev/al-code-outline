using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Formatters;
using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal abstract class ObjectExtensionSymbolFactory<T> : ObjectSymbolFactory<T> where T : ObjectExtensionSymbol
    {

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);

            node.Extends = DisplayStringFormatter.FormatFullyQualifiedName(symbol.ExtendedObjectReference.FullyQualifiedName);

            return node;
        }

    }
}
