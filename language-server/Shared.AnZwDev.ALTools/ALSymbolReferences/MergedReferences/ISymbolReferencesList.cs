using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.MergedReferences
{
    public interface ISymbolReferencesList : IReadOnlyList<ALAppSymbolReference>
    {

        bool InternalsVisible(int index);

    }
}
