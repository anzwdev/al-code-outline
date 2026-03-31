using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class RequestPageExtensionSymbol : Symbol
    {

        public required PropertySymbolsCollection? Properties { get; init; }
        public required List<GlobalVariableDeclarationSymbol>? Variables { get; init; }
        public required List<MethodSymbol>? Methods { get; init; }
        public required List<PageControlChangeSymbol>? ControlChanges { get; init; }
        public required List<PageActionChangeSymbol>? ActionChanges { get; init; }

    }
}
