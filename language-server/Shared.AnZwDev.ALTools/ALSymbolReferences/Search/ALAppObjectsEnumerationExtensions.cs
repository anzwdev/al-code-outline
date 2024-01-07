using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.Search
{
    public static class ALAppObjectsEnumerationExtensions
    {

        public static IEnumerable<int> GetIds<T>(IEnumerable<T> objects) where T : ALAppObject
        {
            foreach (T obj in objects)
                yield return obj.Id;
        }

    }
}
