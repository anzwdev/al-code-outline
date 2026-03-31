using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Formatters;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal class VariableDeclarationSymbolFactory<T> : NamedSymbolFactory<T> where T : VariableDeclarationSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.VariableDeclaration;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);
            node.FullName = DisplayStringFormatter.FormatVariableDeclaration(symbol);

            return node;
        }

    }
}
