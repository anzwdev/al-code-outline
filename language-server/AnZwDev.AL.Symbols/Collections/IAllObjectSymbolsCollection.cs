using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{
    public interface IAllObjectSymbolsCollection
    {

        public IEnumerable<ObjectSymbol> Filter(HashSet<ObjectKind>? objectTypeFilter, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public ObjectSymbol? FindFirst(ObjectReference reference, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public ObjectSymbol? FindFirst(ObjectIdentifier identifier, AccessLevelFilter accessLevelFilter);
        public ObjectSymbol? FindFirst(ObjectKind objectType, int id, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public IEnumerable<ObjectSymbol> FindAll(ObjectReference reference, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);

    }
}
