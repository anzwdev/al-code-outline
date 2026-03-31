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

    internal class PermissionSetExtensionSymbolFactory : PermissionSetExtensionSymbolFactory<PermissionSetExtensionSymbol>
    {
    }

    internal class PermissionSetExtensionSymbolFactory<T> : ObjectExtensionSymbolFactory<T> where T : PermissionSetExtensionSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.PermissionSetExtension;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            CollectionSymbolFactory.Append(node, symbol.Permissions, SymbolFactoryInstances.PermissionSymbolFactory);
            base.CreateChildNodes(node, symbol);
        }

    }

}
