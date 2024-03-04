using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public interface IALAppObjectsCollection : IList<ALAppObject>
    {

        void Replace(ALAppObject alAppObject);

        ALAppObject FindFirst(string namespaceName, string name);
        ALAppObject FindFirst(ALObjectReference objectReference);

        bool UsesNamespaces();

    }
}
