using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppEnum : ALAppObject
    {

        public ALAppSymbolsCollection<ALAppEnumValue> Values { get; set; }

        public ALAppEnum()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.EnumType;
        }

        public override ALObjectType GetALObjectType()
        {
            return ALObjectType.EnumType;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Values?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
