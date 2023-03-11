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

        public ALAppElementsCollection<ALAppPageControl> Controls { get; set; }
        public ALAppElementsCollection<ALAppPageAction> Actions { get; set; }

        public ALAppPage()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.PageObject;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Controls?.AddToALSymbol(symbol, ALSymbolKind.PageLayout, "layout");
            this.Actions?.AddToALSymbol(symbol, ALSymbolKind.PageActionList, "actions");
            base.AddChildALSymbols(symbol);
        }

        public string GetSourceTable()
        {
            if (this.Properties != null)
            {
                ALAppProperty sourceTable = this.Properties.GetProperty("SourceTable");
                if (sourceTable != null)
                    return sourceTable.Value;
            }
            return null;
        }

        public override void ReplaceIdReferences(ALAppObjectIdMap idMap)
        {
            if (this.Properties != null)
            {
                ALAppProperty sourceTable = this.Properties.GetProperty("SourceTable");
                int id;
                if ((sourceTable != null) && (!String.IsNullOrWhiteSpace(sourceTable.Value)) && (Int32.TryParse(sourceTable.Value, out id)))
                {
                    if (idMap.TableIdMap.ContainsKey(id))
                        sourceTable.Value = idMap.TableIdMap[id].Name;
                }
            }
        }

    }
}
