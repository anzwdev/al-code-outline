using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SymbolsTreeViews.SymbolFactories
{

    internal class ReportLabelSymbolFactory : ReportLabelSymbolFactory<ReportLabelSymbol>
    {
    }

    internal class ReportLabelSymbolFactory<T> : NamedSymbolWithIdFactory<T> where T : ReportLabelSymbol
    {

        protected override ALSyntaxNodeKind GetKind(T symbol)
        {
            return ALSyntaxNodeKind.ReportLabel;
        }

    }
}
