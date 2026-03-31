using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class PermissionSymbol : Symbol
    {

        public required ObjectReference ObjectReference { get; init; }
        public required PermissionValue Value { get; init; }

    }
}
