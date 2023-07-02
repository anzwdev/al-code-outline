using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public static class LabelInformationList_Extensions
    {

        public static bool ContainsValue(this List<LabelInformation> elements, string value)
        {
            for (int i = 0; i < elements.Count; i++)
                if (elements[i].Value == value)
                    return true;
            return false;
        }

    }
}
