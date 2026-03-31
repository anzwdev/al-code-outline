using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{
    public class ReadOnlyObjectSymbolsCollection<T> : IObjectSymbolsCollection<T>, IObjectSymbolsCollection where T : ObjectSymbol
    {

        private ObjectSymbolsCollection<T> _items;

        public ReadOnlyObjectSymbolsCollection(ObjectSymbolsCollection<T> items)
        {
            _items = items;
        }

        public void Add(ObjectSymbol symbol)
        {
            throw new NotImplementedException();
        }

        public bool UsesNamespaces()
        {
            return _items.UsesNamespaces();
        }

        public IEnumerable<T> Filter(AccessLevelFilter accessLevelFilter)
        {
            return _items.Filter(accessLevelFilter);
        }

        public IEnumerable<T> FindAll(ObjectReference objectReference, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            return _items.FindAll(objectReference, accessLevelFilter);
        }

        public T? FindFirst(int id, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            return _items.FindFirst(id, accessLevelFilter);
        }

        public T? FindFirst(ObjectReference reference, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            return _items.FindFirst(reference, accessLevelFilter);
        }

        public T? FindFirst(ObjectIdentifier objectIdentifier, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            return _items.FindFirst(objectIdentifier, accessLevelFilter);
        }

        public void RemoveReferenceSourceFileName(string referenceSourceFileName)
        {
            throw new NotImplementedException();
        }

        public void RenameReferenceSourceFileName(string oldReferenceSourceFileName, string newReferenceSourceFileName)
        {
            throw new NotImplementedException();
        }

        IEnumerable<ObjectSymbol> IObjectSymbolsCollection.Filter(AccessLevelFilter accessLevelFilter)
        {
            return _items.Filter(accessLevelFilter);
        }

        IEnumerable<ObjectSymbol> IObjectSymbolsCollection.FindAll(ObjectReference objectReference, AccessLevelFilter accessLevelFilter)
        {
            return _items.FindAll(objectReference, accessLevelFilter);
        }

        ObjectSymbol? IObjectSymbolsCollection.FindFirst(int id, AccessLevelFilter accessLevelFilter)
        {
            return _items.FindFirst(id, accessLevelFilter);
        }

        ObjectSymbol? IObjectSymbolsCollection.FindFirst(ObjectReference reference, AccessLevelFilter accessLevelFilter)
        {
            return _items.FindFirst(reference, accessLevelFilter);
        }

        ObjectSymbol? IObjectSymbolsCollection.FindFirst(ObjectIdentifier objectIdentifier, AccessLevelFilter accessLevelFilter)
        {
            return _items.FindFirst(objectIdentifier, accessLevelFilter);
        }

    }
}
