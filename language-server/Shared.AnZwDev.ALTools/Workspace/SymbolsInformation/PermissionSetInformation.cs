using AnZwDev.ALTools.ALSymbolReferences;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class PermissionSetInformation : BaseObjectInformation
    {

        [JsonProperty("permissions", NullValueHandling = NullValueHandling.Ignore)]
        public List<PermissionInformation> Permissions { get; set; }

        [JsonProperty("effectivePermissions", NullValueHandling = NullValueHandling.Ignore)]
        public List<PermissionInformation> EffectivePermissions { get; set; }

        [JsonProperty("included", NullValueHandling = NullValueHandling.Ignore)]
        public List<PermissionInformation> Included { get; set; }

        [JsonProperty("excluded", NullValueHandling = NullValueHandling.Ignore)]
        public List<PermissionInformation> Excluded { get; set; }

        public PermissionSetInformation()
        {
        }

        public PermissionSetInformation(ALAppPermissionSet permissionSet) : base(permissionSet)
        {
        }

    }
}
