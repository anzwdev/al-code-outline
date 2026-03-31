using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class ControlAddInSymbolFactory : ControlAddInSymbolFactory<ControlAddInSymbol>
    {
    }

    internal class ControlAddInSymbolFactory<T> : ObjectSymbolFactory<T> where T : ControlAddInSymbol
    {
        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.ControlAddInObject;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {

            CollectionSymbolFactory.Append(node, symbol.Events, SymbolFactoryInstances.EventSymbolFactory);
            CollectionSymbolFactory.Append(node, symbol.Methods, SymbolFactoryInstances.MethodSymbolFactory);

            base.CreateChildNodes(node, symbol);
        }

    }

}
