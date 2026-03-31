using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.EnumerableExtensions
{
    internal struct EnumerableStackItem<T>
    {
        public List<T> Items { get; set; }
        public int CurrentItemIndex { get; set; }
    }
}
