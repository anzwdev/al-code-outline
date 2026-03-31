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

    internal class QuerySymbolFactory : QuerySymbolFactory<QuerySymbol>
    {
    }

    internal class QuerySymbolFactory<T> : ObjectWithCodeSymbolFactory<T> where T : QuerySymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.QueryObject;
        }
        
        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Elements, ALSyntaxNodeKind.QueryElements, "elements", SymbolFactoryInstances.QueryDataItemSymbolFactory));

            base.CreateChildNodes(node, symbol);
        }

    }

}
