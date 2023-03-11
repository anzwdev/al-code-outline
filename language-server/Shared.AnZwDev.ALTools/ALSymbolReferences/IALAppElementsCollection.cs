using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public interface IALAppElementsCollection
    {

        void AddBaseElement(ALAppBaseElement element);
        void RemoveBaseElement(ALAppBaseElement element);
        void ReplaceBaseElement(ALAppBaseElement element);

    }
}
