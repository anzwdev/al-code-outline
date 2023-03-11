using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation.Sorting
{
    public class SymbolWithIdInformationIdComparer : IComparer<SymbolWithIdInformation>
    {

        public int Compare(SymbolWithIdInformation x, SymbolWithIdInformation y)
        {
            return x.Id.CompareTo(y.Id);
        }

    }
}
