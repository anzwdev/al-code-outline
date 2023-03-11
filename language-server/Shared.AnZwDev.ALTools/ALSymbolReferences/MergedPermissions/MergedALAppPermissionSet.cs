using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.MergedPermissions
{
    public class MergedALAppPermissionSet : ALAppPermissionSet
    {

        public MergedALAppPermissionsCollection EffectivePermissions { get; } = new MergedALAppPermissionsCollection();
        public MergedALAppPermissionsCollection IncludedPermissions { get; } = new MergedALAppPermissionsCollection();
        public MergedALAppPermissionsCollection ExcludedPermissions { get; } = new MergedALAppPermissionsCollection();

        public MergedALAppPermissionSet()
        {
            this.Id = 0;
            this.Name = "";
        }

        public MergedALAppPermissionSet(ALAppPermissionSet permissionSet)
        {
            this.Id = permissionSet.Id;
            this.Name = permissionSet.Name;
            if (permissionSet.Permissions != null)
                IncludedPermissions.AddRange(permissionSet.Permissions);
        }

        public void UpdateEffectivePermissions()
        {
            EffectivePermissions.Clear();
            EffectivePermissions.AddRange(IncludedPermissions.GetAllPermissions());
            EffectivePermissions.ExcludeRange(ExcludedPermissions.GetAllPermissions());
        }

    }
}
