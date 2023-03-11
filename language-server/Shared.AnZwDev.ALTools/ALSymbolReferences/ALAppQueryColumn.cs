using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppQueryColumn : ALAppElementWithName
    {

        public string SourceColumn { get; set; }

        public ALAppQueryColumn()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.QueryColumn;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol =  base.CreateMainALSymbol();
            if (!String.IsNullOrWhiteSpace(this.SourceColumn))
                symbol.fullName = ALSyntaxHelper.EncodeName(this.Name) + ": " + ALSyntaxHelper.EncodeName(this.SourceColumn);
            return symbol;
        }

    }
}
