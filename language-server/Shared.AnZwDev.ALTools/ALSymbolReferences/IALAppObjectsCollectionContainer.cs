using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public interface IALAppObjectsCollectionContainer
    {

        IALAppObjectsCollection Objects { get; }

        IALAppObjectsCollection GetOrCreateObjectsCollection();

    }
}
