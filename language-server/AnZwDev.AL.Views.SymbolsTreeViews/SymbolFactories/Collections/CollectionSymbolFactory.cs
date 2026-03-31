using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections
{
    internal static class CollectionSymbolFactory
    {

        public static SymbolsTreeNode? Create<T>(IList<T>? list, ALSyntaxNodeKind kind, string name, SymbolFactory<T> symbolFactory) where T : Symbol
        {
            if (list == null || list.Count == 0)
                return null;

            var symbolTreeNode = new SymbolsTreeNode()
            {
                Kind = kind,
                Name = name,
                FullName = name,
                TreeNodeSource = null
            };

            Append(symbolTreeNode, list, symbolFactory);

            return symbolTreeNode;
        }

        public static void Append<T>(SymbolsTreeNode symbolTreeNode, IList<T>? list, SymbolFactory<T> symbolFactory) where T : Symbol
        {
            if (list != null)
                for (int i = 0; i < list.Count; i++)
                    symbolTreeNode.AddChildSymbol(symbolFactory.Create(list[i]));
        }

    }
}
