using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppDotNetTypeDeclaration : ALAppBaseElement
    {

        public string TypeName { get; set; }
        public string AliasName { get; set; }

        public ALAppDotNetTypeDeclaration()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.DotNetTypeDeclaration;
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = new ALSymbol(this.GetALSymbolKind(), this.TypeName);
            if (!String.IsNullOrWhiteSpace(this.AliasName))
                symbol.fullName = ALSyntaxHelper.EncodeName(this.AliasName) + ": " + ALSyntaxHelper.EncodeName(this.TypeName.NotNull());
            return symbol;
        }

    }
}
