using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal abstract class ObjectExtensionWithCodeSymbolFactory<T> : ObjectExtensionSymbolFactory<T> where T : ObjectExtensionWithCodeSymbol
    {

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Variables, ALSyntaxNodeKind.VarSection, "var", SymbolFactoryInstances.GlobalVariableDeclarationSymbolFactory));
            CollectionSymbolFactory.Append(node, symbol.Methods, SymbolFactoryInstances.MethodSymbolFactory);

            base.CreateChildNodes(node, symbol);
        }

    }
}
