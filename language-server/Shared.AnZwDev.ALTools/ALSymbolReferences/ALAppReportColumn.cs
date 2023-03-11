using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppReportColumn : ALAppElementWithName
    {

        public string OwningDataItemName { get; set; }
        public string SourceExpression { get; set; }

        public ALAppReportColumn()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ReportColumn;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            if (!String.IsNullOrWhiteSpace(this.SourceExpression))
                symbol.fullName = ALSyntaxHelper.EncodeName(this.Name) + ": " + this.SourceExpression;
            return symbol;
        }


    }
}
