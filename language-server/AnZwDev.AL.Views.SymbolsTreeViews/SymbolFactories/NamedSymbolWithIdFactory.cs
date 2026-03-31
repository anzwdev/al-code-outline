using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal abstract class NamedSymbolWithIdFactory<T> : NamedSymbolFactory<T> where T : NamedSymbolWithId
    {

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);
           
            node.Id = symbol.Id;

            return node;
        }

    }
}
