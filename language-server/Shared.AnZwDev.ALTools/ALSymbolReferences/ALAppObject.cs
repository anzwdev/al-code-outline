using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppObject : ALAppElementWithNameId
    {
        public bool INT_Parsed { get; set; }
        public string ReferenceSourceFileName { get; set; }
        public ALAppPropertiesCollection Properties { get; set; }
        public ALAppElementsCollection<ALAppVariable> Variables { get; set; }
        public ALAppElementsCollection<ALAppMethod> Methods { get; set; }


        public ALAppObject()
        {
            this.INT_Parsed = false;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.fullName = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(this.Name);
            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Variables?.AddToALSymbol(symbol, ALSymbolKind.VarSection, "var");
            this.Methods?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

        public virtual void ReplaceIdReferences(ALAppObjectIdMap idMap)
        {
        }

        public ALAppAccessMode GetAccessMode()
        {
            if (this.Properties != null)
            {
                string access = this.Properties.GetValue("Access");
                if (!String.IsNullOrWhiteSpace(access))
                {
                    ALAppAccessMode accessModeValue;
                    if (Enum.TryParse<ALAppAccessMode>(access, true, out accessModeValue))
                        return accessModeValue;
                }
            }
            return ALAppAccessMode.Public;
        }

        public bool IsInternal()
        {
            if (this.Properties != null)
            {
                var internalProperty = this.Properties
                    .Where(p => ((p.Name != null) && (p.Name.Equals("Access", StringComparison.CurrentCultureIgnoreCase))))
                    .FirstOrDefault();
                if (internalProperty != null)
                    return ((internalProperty.Value != null) && (internalProperty.Value.Equals("Internal", StringComparison.CurrentCultureIgnoreCase)));
            }
            return false;
        }

    }
}
