using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal abstract class NamedSymbolFactory<T> : SymbolFactory<T> where T : NamedSymbol
    {

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);
            node.Name = symbol.Name;
            node.FullName = kind.ToDescriptionString() + " " + ALLiteralFormatter.GetName(symbol.Name);

            return node;
        }

    }
}
