using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools.ALSymbols;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppVariable : ALAppElementWithName
    {

        public bool Protected { get; set; }
        public ALAppTypeDefinition TypeDefinition { get; set; }

        public ALAppVariable()
        {
        }

        public override ALSymbolAccessModifier? GetAccessModifier()
        {
            if (Protected)
                return ALSymbolAccessModifier.Protected;
            return base.GetAccessModifier();
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.VariableDeclaration;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            if (this.TypeDefinition != null)
                symbol.fullName = ALSyntaxHelper.EncodeName(this.Name) + ": " + this.TypeDefinition.GetSourceCode();
            symbol.access = GetAccessModifier();
            return symbol;
        }

    }
}
