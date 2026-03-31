using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal class ProjectDefinitionSymbolFactory : ProjectDefinitionSymbolFactory<ProjectDefinitionSymbol>
    {
    }

    internal class ProjectDefinitionSymbolFactory<T> : SymbolFactory<T> where T : ProjectDefinitionSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.ProjectDefinition;
        }

        protected override SymbolsTreeNode CreateNode(T symbol, ALSyntaxNodeKind kind)
        {
            var name = String.Join(" ", symbol.Application.Publisher, symbol.Application.Name, symbol.Application.Version.ToString());
            var node = base.CreateNode(symbol, kind);

            node.Name = name;
            node.FullName = name;

            return node;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            base.CreateChildNodes(node, symbol);

            //add dependencies collection
            if (symbol.Dependencies.Count > 0)
                node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Dependencies, ALSyntaxNodeKind.Dependencies, "Dependencies", SymbolFactoryInstances.ApplicationSymbolFactory));

            //add application
            node.AddChildSymbol(SymbolFactoryInstances.ApplicationSymbolFactory.Create(symbol.Application));
        }

    }
}
