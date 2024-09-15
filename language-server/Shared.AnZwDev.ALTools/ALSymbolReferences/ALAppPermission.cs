using AnZwDev.ALTools.ALSymbolReferences.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppPermission : ALAppBaseElement
    {

        public ALAppPermissionObjectType PermissionObject { get; set; }
        public ALAppPermissionValue Value { get; set; }
        public int Id { get; set; }
        public string ObjectName { get; set; }

        public ALAppPermission()
        {
        }

        public string GetObjectNameKey()
        {
            if (ObjectName != null)
                return ObjectName.ToLower();
            return "";
        }

    }
}
