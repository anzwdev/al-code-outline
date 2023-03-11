using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPermissionSet : ALAppObject
    {

        public ALAppElementsCollection<ALAppPermission> Permissions { get; set; }

        public ALAppPermissionSet()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.PermissionSet;
        }

        public override void ReplaceIdReferences(ALAppObjectIdMap idMap)
        {
            if (this.Permissions != null)
                for (int i = 0; i < this.Permissions.Count; i++)
                    this.Permissions[i].ReplaceIdReferences(idMap);
        }

    }
}
