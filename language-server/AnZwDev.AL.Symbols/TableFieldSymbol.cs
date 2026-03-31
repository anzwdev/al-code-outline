using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class TableFieldSymbol : NamedSymbolWithIdAndProperties
    {
        public required TypeDefinitionSymbol? TypeDefinition { get; init; }
    }
}
