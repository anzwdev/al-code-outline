using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Metadata
{
    public class InternalsVisibleToModule : Symbol
    {

        public required string AppId { get; init; }
        public required string? Name { get; init; }
        public required string? Publisher { get; init; }

    }
}
