using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppControlAddIn : ALAppObject
    {

        public ALAppElementsCollection<ALAppMethod> Events { get; set; }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.ControlAddInObject;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Events?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }


    }
}
