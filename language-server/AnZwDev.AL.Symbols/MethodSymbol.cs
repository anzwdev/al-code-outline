using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public partial class MethodSymbol : NamedSymbol, IMemberSymbol
    {

        public required int Id { get; init; }
        public required MemberKind MemberKind { get; init; }
        public required List<AttributeSymbol>? Attributes { get; init; }
        public required bool IsInternal { get; init; }
        public required bool IsLocal { get; init; }
        public required bool IsProtected { get; init; }

        public required List<MethodParameterSymbol>? Parameters { get; init; }
        public required MethodReturnParameterDefinitionSymbol? ReturnParameterDefinition { get; init; }


        public bool IsPublic()
        {
            return !(IsInternal || IsLocal || IsProtected);
        }

    }
}
