using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class GlobalVariableDeclarationSymbol : VariableDeclarationSymbol, IMemberSymbol
    {

        public MemberKind MemberKind { get; } = MemberKind.GlobalVarSection;

        public required bool Protected { get; init; }

    }
}
