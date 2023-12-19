using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public interface IALAppSymbolsCollection
    {

        void AddBaseElement(ALAppBaseElement element);
        void RemoveBaseElement(ALAppBaseElement element);
        void ReplaceBaseElement(ALAppBaseElement element);

    }
}
