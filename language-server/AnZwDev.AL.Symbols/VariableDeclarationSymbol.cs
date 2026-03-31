using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public partial class VariableDeclarationSymbol : NamedSymbol
    {
        public required TypeDefinitionSymbol? TypeDefinition { get; init; }
        public required List<AttributeSymbol>? Attributes { get; init; }

    }
}
