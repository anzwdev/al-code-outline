using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class ReportSymbolFactory : ReportSymbolFactory<ReportSymbol>
    {
    }

    internal class ReportSymbolFactory<T> : ObjectWithCodeSymbolFactory<T> where T : ReportSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.ReportObject;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.DataItems, ALSyntaxNodeKind.ReportDataSetSection, "dataset", SymbolFactoryInstances.ReportDataItemSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Labels, ALSyntaxNodeKind.ReportLabelsSection, "labels", SymbolFactoryInstances.ReportLabelSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Layouts, ALSyntaxNodeKind.ReportRenderingSection, "rendering", SymbolFactoryInstances.ReportLayoutSymbolFactory));

            if (symbol.RequestPage != null)
                node.AddChildSymbol(SymbolFactoryInstances.RequestPageSymbolFactory.Create(symbol.RequestPage));

            base.CreateChildNodes(node, symbol);
        }

    }

}
