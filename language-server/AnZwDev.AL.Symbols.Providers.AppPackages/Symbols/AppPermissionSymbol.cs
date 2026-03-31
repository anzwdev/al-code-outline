using AnZwDev.AL.Symbols.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppPermissionSymbol : AppSerializedSymbol<PermissionSymbol>
    {

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Value")]
        public int Value { get; set; }

        [JsonPropertyName("PermissionObject")]
        public int PermissionObject { get; set; }

        public override PermissionSymbol CreateSymbol(string? ns)
        {
            var objectType = AppEnumConverters.PermissionObjectKindToObjectType(PermissionObject);

            return new PermissionSymbol()
            {
                ObjectReference = new ObjectReference(objectType, null, Id, new FullyQualifiedName(ns, String.Empty), null),
                Value = new PermissionValue()
                {
                    Execute = GetPermissionLevel(Value, AppPermissionValue.Execute, AppPermissionValue.IndirectExecute),
                    Read = GetPermissionLevel(Value, AppPermissionValue.Read, AppPermissionValue.IndirectRead),
                    Insert = GetPermissionLevel(Value, AppPermissionValue.Insert, AppPermissionValue.IndirectInsert),
                    Modify = GetPermissionLevel(Value, AppPermissionValue.Modify, AppPermissionValue.IndirectModify),
                    Delete = GetPermissionLevel(Value, AppPermissionValue.Delete, AppPermissionValue.IndirectDelete)
                }
            };
        }

        private PermissionLevel GetPermissionLevel(int value, int directBit, int indirectBit)
        {
            if ((value & directBit) == directBit)
                return PermissionLevel.Direct;

            if ((value & indirectBit) == indirectBit)
                return PermissionLevel.Indirect;

            return PermissionLevel.None;
        }


    }
}
