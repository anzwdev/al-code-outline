using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPageControlChange : ALAppElementWithName
    {

        public ALAppElementsCollection<ALAppPageControl> Controls { get; set; }

        public ALAppPageControlChange()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ControlModifyChange;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Controls?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
