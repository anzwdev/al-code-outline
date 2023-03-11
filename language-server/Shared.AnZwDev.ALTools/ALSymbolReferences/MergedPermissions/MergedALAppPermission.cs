using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.MergedPermissions
{
    public class MergedALAppPermission : ALAppPermission
    {

        public MergedALAppPermission()
        {
        }

        public MergedALAppPermission(ALAppPermission permission)
        {
            PermissionObject = permission.PermissionObject;
            Value = permission.Value;
            Id = permission.Id;
            ObjectName = permission.ObjectName;
        }

        public void Merge(ALAppPermission permission)
        {
            this.Value = this.Value | permission.Value;
        }

        public void Exclude(ALAppPermission permission)
        {
            ALAppPermissionValue newValue = Value & (~permission.Value);
            ALAppPermissionValue indirectRemoval = (permission.Value & ALAppPermissionValue.AllIndirect);
            ALAppPermissionValue newIndirect = Value.DirectToIndirect() & indirectRemoval;
            Value = newValue | newIndirect;
        }

    }
}
