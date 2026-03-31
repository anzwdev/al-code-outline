using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{
    public interface IObjectExtensionSymbolsCollection<T> : IObjectSymbolsCollection<T> where T : ObjectExtensionSymbol
    {

        public IEnumerable<T> FindExtensions(ObjectIdentifier objectIdentifier, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);

    }
}
