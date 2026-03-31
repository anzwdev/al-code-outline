using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols
{
    public enum PropertyKind
    {
        
        Undefined = 0,

        Access = 1,
        Caption = 2,
        Description = 3,
        ToolTip = 4,

        Enabled = 5,
        FieldClass = 6,
        ObsoleteState = 7,
        ObsoleteReason = 8,

        SourceExpression = 9,
        SourceTable = 10,

        IncludedPermissionSets = 11,
        ExcludedPermissionSets = 12,

        InherentPermissions = 13

    }
}
