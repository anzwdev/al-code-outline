using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public struct PermissionValue
    {

        public PermissionLevel Execute { get; set; }
        public PermissionLevel Read { get; set; }
        public PermissionLevel Insert { get; set; }
        public PermissionLevel Modify { get; set; }
        public PermissionLevel Delete { get; set; }

        public PermissionValue Add(PermissionValue other)
        {
            return new PermissionValue
            {
                Execute = Execute.Add(other.Execute),
                Read = Read.Add(other.Read),
                Insert = Insert.Add(other.Insert),
                Modify = Modify.Add(other.Modify),
                Delete = Delete.Add(other.Delete)
            };
        }

        public PermissionValue Remove(PermissionValue other)
        {
            return new PermissionValue
            {
                Execute = Execute.Remove(other.Execute),
                Read = Read.Remove(other.Read),
                Insert = Insert.Remove(other.Insert),
                Modify = Modify.Remove(other.Modify),
                Delete = Delete.Remove(other.Delete)
            };
        }

    }
}
