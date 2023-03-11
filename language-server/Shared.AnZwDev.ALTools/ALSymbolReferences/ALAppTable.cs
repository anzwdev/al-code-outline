using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppTable : ALAppObject
    {


        public ALAppElementsCollection<ALAppTableField> Fields { get; set; }
        public ALAppElementsCollection<ALAppTableKey> Keys { get; set; }
        public ALAppElementsCollection<ALAppFieldGroup> FieldGroups { get; set; }

        public ALAppTable()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.TableObject;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Fields?.AddToALSymbol(symbol, ALSymbolKind.FieldList, "fields");
            this.Keys?.AddToALSymbol(symbol, ALSymbolKind.KeyList, "keys", ALSymbolKind.PrimaryKey);
            this.FieldGroups?.AddToALSymbol(symbol, ALSymbolKind.FieldGroupList, "fieldgroups");
            base.AddChildALSymbols(symbol);
        }

        public ALAppTableKey GetPrimaryKey()
        {
            if ((Keys != null) && (Keys.Count > 0))
                return Keys[0];
            return null;
        }

    }
}
