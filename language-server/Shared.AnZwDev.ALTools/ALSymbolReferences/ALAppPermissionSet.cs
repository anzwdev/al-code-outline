using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPermissionSet : ALAppObject
    {

        public ALAppSymbolsCollection<ALAppPermission> Permissions { get; set; }

        public ALAppPermissionSet()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.PermissionSet;
        }

        public override ALObjectType GetALObjectType()
        {
            return ALObjectType.PermissionSet;
        }

    }
}
