using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal abstract class AppSerializedSymbol<T> where T : Symbol
    {

        public abstract T CreateSymbol(string? ns);

    }
}
