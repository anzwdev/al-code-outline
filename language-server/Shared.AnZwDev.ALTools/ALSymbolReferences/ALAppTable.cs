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


        public ALAppSymbolsCollection<ALAppTableField> Fields { get; set; }
        public ALAppSymbolsCollection<ALAppTableKey> Keys { get; set; }
        public ALAppSymbolsCollection<ALAppFieldGroup> FieldGroups { get; set; }

        public ALAppTable()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.TableObject;
        }

        public override ALObjectType GetALObjectType()
        {
            return ALObjectType.Table;
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

        public override bool HasFullInherentPermissions()
        {
            var permissions = GetInherentPermissions();
            return (permissions != null) &&
                (permissions.IndexOf("R") >= 0) &&
                (permissions.IndexOf("M") >= 0) &&
                (permissions.IndexOf("I") >= 0) &&
                (permissions.IndexOf("D") >= 0) &&
                (permissions.IndexOf("X") >= 0);
        }

    }
}
