using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public partial class SubtypeSymbol : Symbol
    {

        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string? ModuleId { get; init; }

        public bool IsEmpty()
        {
            return (String.IsNullOrWhiteSpace(this.Name) || (this.Name.ToLower() == "none"));
        }

    }
}
