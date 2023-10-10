using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class ListExtensions
    {

        public static List<T> AddOrCreate<T>(this List<T> list, T item)
        {
            if (list == null)
                list = new List<T>();
            list.Add(item);
            return list;
        }

    }
}
