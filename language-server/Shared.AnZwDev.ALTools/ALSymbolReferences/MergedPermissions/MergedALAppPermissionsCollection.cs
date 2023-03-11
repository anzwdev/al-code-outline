using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace AnZwDev.ALTools.ALSymbolReferences.MergedPermissions
{
    public class MergedALAppPermissionsCollection
    {

        private Dictionary<ALAppPermissionObjectType, Dictionary<string, MergedALAppPermission>> _permissionsDictionary = new Dictionary<ALAppPermissionObjectType, Dictionary<string, MergedALAppPermission>>();

        public void Clear()
        {
            _permissionsDictionary.Clear();
        }

        public void Add(ALAppPermission permission)
        {
            var key = permission.GetObjectNameKey();
            var objectTypePermissions = GetObjectTypePermissions(permission.PermissionObject);
            if (objectTypePermissions.ContainsKey(key))
                objectTypePermissions[key].Merge(permission);
            else
                objectTypePermissions.Add(key, new MergedALAppPermission(permission));
        }

        public void Exclude(ALAppPermission permission)
        {
            var key = permission.GetObjectNameKey();
            var objectTypePermissions = GetObjectTypePermissions(permission.PermissionObject);
            if (objectTypePermissions.ContainsKey(key))
            {
                var existingPermission = objectTypePermissions[key];
                existingPermission.Exclude(permission);
                if (existingPermission.Value == ALAppPermissionValue.Empty)
                    objectTypePermissions.Remove(key);
            }
        }

        public void AddRange(IEnumerable<ALAppPermission> permissionsCollection)
        {
            foreach (var permission in permissionsCollection)
                Add(permission);
        }

        public void ExcludeRange(IEnumerable<ALAppPermission> permissionsCollection)
        {
            foreach (var permission in permissionsCollection)
                Exclude(permission);
        }

        private Dictionary<string, MergedALAppPermission> GetObjectTypePermissions(ALAppPermissionObjectType objectType)
        {
            if (_permissionsDictionary.ContainsKey(objectType))
                return _permissionsDictionary[objectType];
            Dictionary<string, MergedALAppPermission> objectTypePermissions = new Dictionary<string, MergedALAppPermission>();
            _permissionsDictionary.Add(objectType, objectTypePermissions);
            return objectTypePermissions;
        }

        public IEnumerable<MergedALAppPermission> GetAllPermissions()
        {
            foreach (var objectTypePermissions in _permissionsDictionary.Values)
                foreach (var permission in objectTypePermissions.Values)
                    yield return permission;
        }

        public bool IsEmpty()
        {
            if (_permissionsDictionary.Count > 0)
                foreach (var value in _permissionsDictionary.Values)
                    if (value.Count > 0)
                        return false;
            return true;
        }

    }
}
