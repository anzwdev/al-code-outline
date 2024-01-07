using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPermissionSetExtension : ALAppObject, IALAppObjectExtension
    {

        public string TargetObject { get; set; }
        public ALAppSymbolsCollection<ALAppPermission> Permissions { get; set; }

        public ALAppPermissionSetExtension()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.PermissionSetExtension;
        }

        public override ALObjectType GetALObjectType()
        {
            return ALObjectType.PermissionSetExtension;
        }


        public string GetTargetObjectName()
        {
            return this.TargetObject;
        }

        public ALObjectReference GetTargetObjectReference()
        {
            return new ALObjectReference(Usings, this.TargetObject);
        }

        public override void ReplaceIdReferences(ALAppObjectIdMap idMap)
        {
            if (this.Permissions != null)
                for (int i=0; i<this.Permissions.Count; i++)
                    this.Permissions[i].ReplaceIdReferences(idMap);
        }

    }
}
