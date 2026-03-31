using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class DotNetTypeDeclarationSymbol : Symbol
    {

        public required string? TypeName { get; init; }
        public required string? AliasName { get; init; }
        public required string? ReferenceSourceFileName { get; init; }

    }
}
