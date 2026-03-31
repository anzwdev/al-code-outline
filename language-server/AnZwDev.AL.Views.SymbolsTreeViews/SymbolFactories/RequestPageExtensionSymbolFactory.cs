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

    internal class RequestPageExtensionSymbolFactory : RequestPageExtensionSymbolFactory<RequestPageExtensionSymbol>
    {
    }

    internal class RequestPageExtensionSymbolFactory<T> : SymbolFactory<T> where T : RequestPageExtensionSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.RequestPageExtension;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.ControlChanges, ALSyntaxNodeKind.PageExtensionLayout, "layout", SymbolFactoryInstances.PageControlChangeSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.ActionChanges, ALSyntaxNodeKind.PageExtensionActionList, "actions", SymbolFactoryInstances.PageActionChangeSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Variables, ALSyntaxNodeKind.VarSection, "var", SymbolFactoryInstances.GlobalVariableDeclarationSymbolFactory));
            CollectionSymbolFactory.Append(node, symbol.Methods, SymbolFactoryInstances.MethodSymbolFactory);

            base.CreateChildNodes(node, symbol);
        }

    }
}
