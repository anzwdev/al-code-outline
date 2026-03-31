using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public partial class TypeDefinitionSymbol : NamedSymbol
    {

        public required SubtypeSymbol? Subtype { get; init; }
        public required List<string>? OptionMembers { get; init; }
        public required bool Temporary { get; init; }
        public required List<int>? ArrayDimensions { get; init; }
        public required List<TypeDefinitionSymbol>? TypeArguments { get; init; }


        public bool IsEmpty()
        {
            return ((String.IsNullOrWhiteSpace(this.Name)) || (this.Name.ToLower() == "none"));
        }

    }
}
