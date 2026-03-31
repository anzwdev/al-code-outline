using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class NamedSymbolWithId : NamedSymbol
    {

        public required int Id { get; init; }

    }
}
