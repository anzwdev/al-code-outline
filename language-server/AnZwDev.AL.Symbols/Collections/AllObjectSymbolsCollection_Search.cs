using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{
    public partial class AllObjectSymbolsCollection : IAllObjectSymbolsCollection
    {

        public IEnumerable<ObjectSymbol> Filter(HashSet<ObjectKind>? objectTypeFilter, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            foreach (var objectType in _objectCollections.Keys)
                if ((objectTypeFilter == null) || (objectTypeFilter.Count == 0) || (objectTypeFilter.Contains(objectType)))
                {
                    var collection = _objectCollections[objectType](ApplicationSymbol);
                    foreach (var symbol in collection.Filter(accessLevelFilter))
                        yield return symbol;
                }
        }

        public IEnumerable<ObjectSymbol> Filter(ObjectKind objectTypeFilter, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            if (_objectCollections.ContainsKey(objectTypeFilter))
            {
                var collection = _objectCollections[objectTypeFilter](ApplicationSymbol);
                foreach (var symbol in collection.Filter(accessLevelFilter))
                    yield return symbol;
            }
        }

        public ObjectSymbol? FindFirst(ObjectReference reference, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            if (_objectCollections.ContainsKey(reference.ObjectKind))
            {
                var collection = _objectCollections[reference.ObjectKind](ApplicationSymbol);
                return collection.FindFirst(reference, accessLevelFilter);
            }
            return null;
        }

        public ObjectSymbol? FindFirst(ObjectIdentifier identifier, AccessLevelFilter accessLevelFilter)
        {
            if (_objectCollections.ContainsKey(identifier.ObjectKind))
            {
                var collection = _objectCollections[identifier.ObjectKind](ApplicationSymbol);
                return collection.FindFirst(identifier, accessLevelFilter);
            }
            return null;
        }

        public ObjectSymbol? FindFirst(ObjectKind objectType, int id, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            if (_objectCollections.ContainsKey(objectType))
            {
                var collection = _objectCollections[objectType](ApplicationSymbol);
                return collection.FindFirst(id, accessLevelFilter);
            }
            return null;
        }

        public IEnumerable<ObjectSymbol> FindAll(ObjectReference reference, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            if (_objectCollections.ContainsKey(reference.ObjectKind))
            {
                var collection = _objectCollections[reference.ObjectKind](ApplicationSymbol);
                return collection.FindAll(reference, accessLevelFilter); 
            }        
            return Enumerable.Empty<ObjectSymbol>();
        }

    }
}
