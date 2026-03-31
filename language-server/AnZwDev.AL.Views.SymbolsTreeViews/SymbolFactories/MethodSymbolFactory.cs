using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AnZwDev.AL.Symbols.Formatters;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class MethodSymbolFactory : MethodSymbolFactory<MethodSymbol>
    {
    }

    internal class MethodSymbolFactory<T> : NamedSymbolFactory<T> where T : MethodSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.MethodDeclaration;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var node = base.CreateNode(symbol, kind);
            node.FullName = DisplayStringFormatter.FormatMethodSymbol(symbol);
            return node;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Parameters, ALSyntaxNodeKind.ParameterList, "parameters", SymbolFactoryInstances.MethodParameterSymbolFactory));

            base.CreateChildNodes(node, symbol);
        }

    }
}
