using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public static class PermissionLevelExtensions
    {

        public static PermissionLevel Add(this PermissionLevel level1, PermissionLevel level2)
        {
            if (
                (level1 == PermissionLevel.None) ||
                ((level1 == PermissionLevel.Indirect) && (level2 == PermissionLevel.Direct))
            )
                return level2;
            return level1;
        }

        public static PermissionLevel Remove(this PermissionLevel level1, PermissionLevel level2)
        {
            if (level2 == PermissionLevel.Direct)
                return PermissionLevel.None;
            if ((level2 == PermissionLevel.Indirect) && (level1 != PermissionLevel.Direct))
                return PermissionLevel.None;
            return level1;
        }

    }
}
