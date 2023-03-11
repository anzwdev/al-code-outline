using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public interface IReadOnlyALAppObjectsCollection
    {

        ALSymbolKind ALSymbolKind { get; }
        IEnumerable<ALAppObject> GetObjects();
        IEnumerable<long> GetIds();

    }
}
