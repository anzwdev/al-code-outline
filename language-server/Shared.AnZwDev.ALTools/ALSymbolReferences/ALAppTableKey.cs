using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppTableKey : ALAppElementWithName
    {

        public string[] FieldNames { get; set; }

        public ALAppTableKey()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.Key;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.fullName = ALSyntaxHelper.EncodeName(this.Name) + ": " + ALSyntaxHelper.EncodeNamesList(this.FieldNames);
            return symbol;
        }

    }
}
