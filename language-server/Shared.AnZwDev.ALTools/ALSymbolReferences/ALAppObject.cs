using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppObject : ALAppElementWithNameId
    {
        public bool INT_Parsed { get; set; }
        public string NamespaceName { get; set; }
        public HashSet<string> Usings { get; set; }

        public string ReferenceSourceFileName { get; set; }
        public ALAppPropertiesCollection Properties { get; set; }
        public ALAppSymbolsCollection<ALAppVariable> Variables { get; set; }
        public ALAppSymbolsCollection<ALAppMethod> Methods { get; set; }


        public ALAppObject()
        {
            this.INT_Parsed = false;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.fullName = symbol.kind.ToName() + " " + ALSyntaxHelper.EncodeName(this.Name);

            symbol.namespaceName = this.NamespaceName;
            symbol.usings = this.Usings;

            return symbol;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.Variables?.AddToALSymbol(symbol, ALSymbolKind.VarSection, "var");
            this.Methods?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

        public ALAppAccessMode GetAccessMode()
        {
            if (this.Properties != null)
            {
                string access = this.Properties.GetNameValue("Access");
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
                    .Where(p => ((p.Name != null) && (p.Name.Equals("Access", StringComparison.OrdinalIgnoreCase))))
                    .FirstOrDefault();
                if (internalProperty != null)
                    return internalProperty.Equals("Internal");
            }
            return false;
        }

        public string GetInherentPermissions()
        {
            return Properties?.GetRawValue("InherentPermissions");
        }

        public virtual bool HasFullInherentPermissions()
        {
            var permissions = GetInherentPermissions();
            return (permissions != null) && (permissions.IndexOf("X") >= 0);
        }

        public ALObjectReference GetReference()
        {
            return new ALObjectReference(Usings, NamespaceName, Id, Name);
        }

        public ALObjectIdentifier GetIdentifier()
        {
            return new ALObjectIdentifier(NamespaceName, Id, Name);
        }

        public string GetFullName()
        {
            return ALSyntaxHelper.EncodeFullName(NamespaceName, Name);
        }

    }
}
