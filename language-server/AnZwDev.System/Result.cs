using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System
{
    public struct Result<T>
    {

        public bool Success { get; set; }
        public T? Value { get; set; }

    }
}
