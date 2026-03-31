using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public class XmlPortTableElementSymbol : XmlPortNodeSymbol
    {

        public required ObjectReference? SourceTable { get; init; }

    }
}
