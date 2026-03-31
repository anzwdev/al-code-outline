using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.Extensions
{
    public static class ArrayExtensions
    {

        public static List<T>? ToListOrNull<T>(this T[]? array)
        {
            if (array == null)
                return null;
            return array.ToList();
        }

    }
}
