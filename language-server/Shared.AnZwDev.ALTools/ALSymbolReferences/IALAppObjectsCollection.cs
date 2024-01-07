using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public interface IALAppObjectsCollection : IList<ALAppObject>
    {

        void Replace(ALAppObject alAppObject);

    }
}
