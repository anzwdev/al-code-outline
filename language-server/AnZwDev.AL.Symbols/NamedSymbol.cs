using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public abstract class NamedSymbol : Symbol
    {

        public required string Name { get; init; }

    }
}
