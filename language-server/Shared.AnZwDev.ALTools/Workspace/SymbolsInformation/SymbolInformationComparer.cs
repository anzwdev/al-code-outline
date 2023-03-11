using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class SymbolInformationComparer : IComparer<SymbolInformation>
    {
        public int Compare(SymbolInformation x, SymbolInformation y)
        {
            if ((x != null) && (y != null))
                return String.Compare(x.Name, y.Name);
            if (x != null)
                return -1;
            if (y != null)
                return 1;
            return 0;
        }
    }
}
