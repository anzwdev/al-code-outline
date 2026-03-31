using AnZwDev.AL.Symbols;
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

    internal class DotNetAssemblyDeclarationSymbolFactory : DotNetAssemblyDeclarationSymbolFactory<DotNetAssemblyDeclarationSymbol>
    {
    }

    internal class DotNetAssemblyDeclarationSymbolFactory<T> : NamedSymbolWithPropertiesFactory<T> where T : DotNetAssemblyDeclarationSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.DotNetAssembly;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            CollectionSymbolFactory.Append(node, symbol.TypeDeclarations, SymbolFactoryInstances.DotNetTypeDeclarationSymbolFactory);
            base.CreateChildNodes(node, symbol);
        }
    }
}
