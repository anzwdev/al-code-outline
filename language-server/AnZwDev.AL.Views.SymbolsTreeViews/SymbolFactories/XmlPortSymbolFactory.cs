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

    internal class XmlPortSymbolFactory : XmlPortSymbolFactory<XmlPortSymbol>
    {
    }

    internal class XmlPortSymbolFactory<T> : ObjectWithCodeSymbolFactory<T> where T : XmlPortSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.XmlPortObject;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Schema, ALSyntaxNodeKind.XmlPortSchema, "schema", SymbolFactoryInstances.XmlPortNodeSymbolFactory));

            if (symbol.RequestPage != null)
                node.AddChildSymbol(SymbolFactoryInstances.RequestPageSymbolFactory.Create(symbol.RequestPage));

            base.CreateChildNodes(node, symbol);
        }

    }

}
