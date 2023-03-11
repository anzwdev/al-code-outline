using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPageActionChange : ALAppElementWithName
    {

        public ALAppElementsCollection<ALAppPageAction> Actions { get; set; }

        public ALAppPageActionChange()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ActionModifyChange;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Actions?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
