using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    
    internal abstract class SymbolFactory<T> where T : Symbol
    {

        public SymbolsTreeNode Create(T symbol)
        {
            var kind = GetKind(symbol);
            var node = CreateNode(symbol, kind);
            CreateChildNodes(node, symbol);
            return node;
        }

        protected abstract ALSyntaxNodeKind GetKind(T symbol);

        protected virtual SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var name = kind.ToDescriptionString();

            return new SymbolsTreeNode()
            {
                Name = name,
                Kind = kind,
                FullName = name,
                TreeNodeSource = symbol
            };
        }

        protected virtual void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
        }

    }

}
