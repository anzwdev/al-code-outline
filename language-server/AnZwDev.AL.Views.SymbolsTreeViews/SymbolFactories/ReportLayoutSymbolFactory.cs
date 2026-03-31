using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class ReportLayoutSymbolFactory : ReportLayoutSymbolFactory<ReportLayoutSymbol>
    {
    }

    internal class ReportLayoutSymbolFactory<T> : NamedSymbolWithPropertiesFactory<T> where T : ReportLayoutSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.ReportLayout;
        }

        protected override void CreateChildNodes(SymbolsTreeNode node, T symbol)
        {


            base.CreateChildNodes(node, symbol);
        }

    }
}
