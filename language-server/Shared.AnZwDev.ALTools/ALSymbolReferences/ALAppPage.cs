using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPage : ALAppObject
    {

        public ALAppSymbolsCollection<ALAppPageControl> Controls { get; set; }
        public ALAppSymbolsCollection<ALAppPageAction> Actions { get; set; }

        public ALAppPage()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.PageObject;
        }

        public override ALObjectType GetALObjectType()
        {
            return ALObjectType.Page;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Controls?.AddToALSymbol(symbol, ALSymbolKind.PageLayout, "layout");
            this.Actions?.AddToALSymbol(symbol, ALSymbolKind.PageActionList, "actions");
            base.AddChildALSymbols(symbol);
        }

        public ALObjectReference GetSourceTable()
        {
            if (this.Properties != null)
            {
                ALAppProperty sourceTable = this.Properties.GetProperty("SourceTable");
                if (sourceTable != null)
                    return new ALObjectReference(Usings, sourceTable.Value);
            }
            return new ALObjectReference();
        }

    }
}
