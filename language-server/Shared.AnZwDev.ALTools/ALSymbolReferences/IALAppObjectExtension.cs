using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public interface IALAppObjectExtension
    {

        string GetTargetObjectName();
        ALObjectReference GetTargetObjectReference();

    }
}
