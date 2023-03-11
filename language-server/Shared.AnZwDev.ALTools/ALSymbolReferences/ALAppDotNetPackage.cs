using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppDotNetPackage : ALAppObject
    {

        public ALAppElementsCollection<ALAppDotNetAssemblyDeclaration> AssemblyDeclarations { get; set; }

        public ALAppDotNetPackage()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.DotNetPackage;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.AssemblyDeclarations?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
