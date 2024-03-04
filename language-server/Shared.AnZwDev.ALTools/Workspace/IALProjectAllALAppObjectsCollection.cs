using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace
{
    public interface IALProjectAllALAppObjectsCollection
    {

        ALAppObject FindFirst(ALObjectReference reference, bool includeInaccessible = false);
        IEnumerable<ALAppObject> GetAll(bool includeInaccessible = false);

    }
}
