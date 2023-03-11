using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppFieldGroup : ALAppElementWithNameId
    {

        public string[] FieldNames { get; set; }

        public ALAppFieldGroup()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.FieldGroup;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.fullName = ALSyntaxHelper.EncodeName(this.Name) + ": " + ALSyntaxHelper.EncodeNamesList(this.FieldNames);
            return symbol;
        }


    }
}
