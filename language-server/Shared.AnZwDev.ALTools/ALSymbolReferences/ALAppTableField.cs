using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppTableField : ALAppElementWithNameId
    {

        public ALAppTypeDefinition TypeDefinition { get; set; }
        public ALAppPropertiesCollection Properties { get; set; }

        public ALAppTableField()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.Field;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            if (this.TypeDefinition != null)
                symbol.fullName = ALSyntaxHelper.EncodeName(this.Name) + ": " + this.TypeDefinition.GetSourceCode();
            this.UpdateSymbolSubtype(symbol);
            return symbol;
        }

        protected void UpdateSymbolSubtype(ALSymbol symbol)
        {
            //detect subtype
            if (this.Properties != null)
            {
                ALAppTableFieldState fieldState = this.GetFieldState();

                if (fieldState == ALAppTableFieldState.Disabled)
                {
                    symbol.subtype = "Disabled";
                    symbol.fullName = symbol.fullName + " (Disabled)";
                    return;
                }

                if ((fieldState == ALAppTableFieldState.ObsoletePending) || (fieldState == ALAppTableFieldState.ObsoleteRemoved))
                {
                    string obsoleteReasonText = "";
                    string obsoleteReason = this.Properties.GetValue("ObsoleteReason");
                    if (!String.IsNullOrWhiteSpace(obsoleteReason))
                        obsoleteReasonText = ": " + obsoleteReason.Trim();

                    switch (fieldState)
                    {
                        case ALAppTableFieldState.ObsoletePending:
                            symbol.subtype = "ObsoletePending";
                            symbol.fullName = symbol.fullName + " (Obsolete-Pending" + obsoleteReasonText + ")";
                            break;
                        case ALAppTableFieldState.ObsoleteRemoved:
                            symbol.subtype = "ObsoleteRemoved";
                            symbol.fullName = symbol.fullName + " (Obsolete-Removed" + obsoleteReasonText + ")";
                            break;
                    }
                }
            }
        }

        public ALAppTableFieldState GetFieldState()
        {
            if (this.Properties != null)
            {
                string enabledState = this.Properties.GetValue("Enabled");
                if ((enabledState != null) && (enabledState.Equals("0") || enabledState.Equals("false", StringComparison.CurrentCultureIgnoreCase)))
                    return ALAppTableFieldState.Disabled;

                string obsoleteState = this.Properties.GetValue("ObsoleteState");
                if (!String.IsNullOrWhiteSpace(obsoleteState))
                {
                    if (obsoleteState.Equals("Pending", StringComparison.CurrentCultureIgnoreCase))
                        return ALAppTableFieldState.ObsoletePending;
                    if (obsoleteState.Equals("Removed", StringComparison.CurrentCultureIgnoreCase))
                        return ALAppTableFieldState.ObsoleteRemoved;
                }
            }
            return ALAppTableFieldState.Active;
        }

        public ALAppTableFieldClass GetFieldClass()
        {
            if (this.Properties != null)
            {
                string fieldClass = this.Properties.GetValue("FieldClass");
                if (!String.IsNullOrWhiteSpace(fieldClass))
                {
                    if (fieldClass.Equals("FlowField", StringComparison.CurrentCultureIgnoreCase))
                        return ALAppTableFieldClass.FlowField;
                    if (fieldClass.Equals("FlowFilter", StringComparison.CurrentCultureIgnoreCase))
                        return ALAppTableFieldClass.FlowFilter;
                }
            }
            return ALAppTableFieldClass.Normal;
        }

    }
}
