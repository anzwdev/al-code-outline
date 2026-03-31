using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class InterfaceSymbolFactory : InterfaceSymbolFactory<InterfaceSymbol>
    {
    }

    internal class InterfaceSymbolFactory<T> : ObjectSymbolFactory<T> where T : InterfaceSymbol
    {
        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.Interface;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            CollectionSymbolFactory.Append(node, symbol.Methods, SymbolFactoryInstances.MethodSymbolFactory);
            base.CreateChildNodes(node, symbol);
        }
    }

}
