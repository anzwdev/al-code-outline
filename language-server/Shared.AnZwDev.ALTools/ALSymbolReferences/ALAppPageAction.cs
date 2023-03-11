using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPageAction : ALAppElementWithName
    {

        public ALAppPageActionKind Kind { get; set; }
        public ALAppElementsCollection<ALAppPageAction> Actions { get; set; }
        public ALAppPropertiesCollection Properties { get; set; }

        public ALAppPageAction()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return this.Kind.ToALSymbolKind();
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.fullName = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(this.Name);
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Actions?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
