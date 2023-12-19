using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppDotNetAssemblyDeclaration : ALAppElementWithName
    {

        public ALAppSymbolsCollection<ALAppDotNetTypeDeclaration> TypeDeclarations { get; set; }

        public ALAppDotNetAssemblyDeclaration()
        {
        }

        public override ALSymbolKind GetALSymbolKind()
        {
            return ALSymbolKind.DotNetAssembly;
        }

        protected override void AddChildALSymbols(ALSymbol symbol)
        {
            this.TypeDeclarations?.AddToALSymbol(symbol);
            base.AddChildALSymbols(symbol);
        }

    }
}
