using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public abstract class NamedSymbolWithIdAndProperties : NamedSymbolWithId
    {

        public required PropertySymbolsCollection? Properties { get; init; }


    }
}
