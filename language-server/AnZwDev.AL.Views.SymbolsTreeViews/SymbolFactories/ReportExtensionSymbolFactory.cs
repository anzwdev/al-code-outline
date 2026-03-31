using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{
    internal class ReportExtensionSymbolFactory : ReportExtensionSymbolFactory<ReportExtensionSymbol>
    {
    }

    internal class ReportExtensionSymbolFactory<T> : ObjectExtensionWithCodeSymbolFactory<T> where T : ReportExtensionSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.ReportExtensionObject;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {

            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.DataItems, ALSyntaxNodeKind.ReportExtensionAddDataItemChange, "dataset", SymbolFactoryInstances.ReportDataItemSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Columns, ALSyntaxNodeKind.ReportExtensionAddColumnChange, "columns", SymbolFactoryInstances.ReportColumnSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Labels, ALSyntaxNodeKind.ReportLabelsSection, "labels", SymbolFactoryInstances.ReportLabelSymbolFactory));
            node.AddChildSymbol(CollectionSymbolFactory.Create(symbol.Layouts, ALSyntaxNodeKind.ReportRenderingSection, "rendering", SymbolFactoryInstances.ReportLayoutSymbolFactory));

            if (symbol.RequestPage != null)
                node.AddChildSymbol(SymbolFactoryInstances.RequestPageExtensionSymbolFactory.Create(symbol.RequestPage));

            base.CreateChildNodes(node, symbol);
        }

    }

}
