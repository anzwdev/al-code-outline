using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppEnumExtension : ALAppEnum, IALAppObjectExtension
    {

        public string TargetObject { get; set; }

        public ALAppEnumExtension()
        {
        }

        protected override ALSymbol CreateMainALSymbol()
        {
            ALSymbol symbol = base.CreateMainALSymbol();
            symbol.extends = this.TargetObject;
            return symbol;
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.EnumExtensionType;
        }

        public string GetTargetObjectName()
        {
            return this.TargetObject;
        }
    }
}
