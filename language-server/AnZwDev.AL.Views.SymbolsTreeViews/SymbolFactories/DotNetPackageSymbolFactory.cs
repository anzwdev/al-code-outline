using AnZwDev.AL.Symbols;
using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class DotNetPackageSymbolFactory : DotNetPackageSymbolFactory<DotNetPackageSymbol>
    {
    }

    internal class DotNetPackageSymbolFactory<T> : ObjectSymbolFactory<T> where T : DotNetPackageSymbol
    {
        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.DotNetPackage;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            CollectionSymbolFactory.Append(node, symbol.AssemblyDeclarations, SymbolFactoryInstances.DotNetAssemblyDeclarationSymbolFactory);
            base.CreateChildNodes(node, symbol);
        }
    }

}
