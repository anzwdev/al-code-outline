using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public static class SymbolInformationListExtensions
    {

        public static Dictionary<string, T> ToDictionary<T>(this List<T> list) where T : SymbolInformation
        {
            Dictionary<string, T> dictionary = new Dictionary<string, T>();
            for (int i=0; i<list.Count; i++)
            {
                dictionary.Add(list[i].Name.ToLower(), list[i]);
            }
            return dictionary;
        }
        

    }
}
