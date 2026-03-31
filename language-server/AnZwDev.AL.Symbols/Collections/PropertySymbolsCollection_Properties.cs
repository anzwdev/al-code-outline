using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{
    public partial class PropertySymbolsCollection
    {

        public bool Enabled
        {
            get
            {
                return GetValue<bool>(PropertyKind.Enabled, true);
            }
        }

        public string? ObsoleteReason
        {
            get
            {
                return GetValue<string?>(PropertyKind.ObsoleteReason, String.Empty);
            }
        }

        public AccessLevel Access
        {
            get
            {
                return GetValue<AccessLevel>(PropertyKind.Access, PropertyValueDefaults.Access);
            }
        }

        public ObsoleteState ObsoleteState
        {
            get
            {
                return GetValue<ObsoleteState>(PropertyKind.ObsoleteState, PropertyValueDefaults.ObsoleteState);
            }
        }

        public FieldClass FieldClass
        {
            get
            {
                return GetValue(PropertyKind.FieldClass, PropertyValueDefaults.FieldClass);
            }
        }

        public string? SourceExpression
        {
            get
            {
                return GetValue<string?>(PropertyKind.SourceExpression, null);
            }
        }

        public string? SourceTable
        {
            get
            {
                return GetValue<string?>(PropertyKind.SourceTable, null);
            }
        }

        public Label Caption
        {
            get
            {
                return GetValue<Label>(PropertyKind.Caption, new Label());
            }
        }

        public Label ToolTip
        {
            get
            {
                return GetValue(PropertyKind.ToolTip, new Label());
            }
        }

        public string? Description
        {
            get
            {
                return GetValue<string?>(PropertyKind.Description, null);
            }
        }

        public string? InherentPermissions
        {
            get
            {
                return GetValue<string?>(PropertyKind.InherentPermissions, null);
            }
        }

        public string? IncludedPermissionSets
        {
            get
            {
                return GetValue<string?>(PropertyKind.IncludedPermissionSets, null);
            }
        }

        public string? ExcludedPermissionSets
        {
            get
            {
                return GetValue<string?>(PropertyKind.ExcludedPermissionSets, null);
            }
        }

    }
}
