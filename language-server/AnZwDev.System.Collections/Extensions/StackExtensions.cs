using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.Collections.Extensions
{
    public static class StackExtensions
    {

        public static void PushReversedRange<T>(this Stack<T> stack, IList<T> items)
        {
            for (int i = items.Count - 1; i >= 0; i--)
                stack.Push(items[i]);
        }

    }
}
