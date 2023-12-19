using AnZwDev.ALTools.ALSymbolReferences;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolReferences
{
    public struct ALProjectSymbolReferenceWithAccessPermissions
    {

        public ALAppSymbolReference Symbols { get; set; }
        public bool InternalsVisible { get; set; }
        public bool PrivateVisible { get; set; }

        public ALProjectSymbolReferenceWithAccessPermissions(ALAppSymbolReference symbols, bool internalsVisible, bool privateVisible)
        {
            this.Symbols = symbols;
            this.InternalsVisible = internalsVisible;
            this.PrivateVisible = privateVisible;
        }

    }
}
