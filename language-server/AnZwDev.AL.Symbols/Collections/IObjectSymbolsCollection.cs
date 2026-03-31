using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{
    public interface IObjectSymbolsCollection
    {

        public void Add(ObjectSymbol symbol);
        public void RemoveReferenceSourceFileName(string referenceSourceFileName);
        public void RenameReferenceSourceFileName(string oldReferenceSourceFileName, string newReferenceSourceFileName);

        public bool UsesNamespaces();

        public IEnumerable<ObjectSymbol> Filter(AccessLevelFilter accessLevelFilter);
        public ObjectSymbol? FindFirst(int id, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public ObjectSymbol? FindFirst(ObjectReference reference, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public ObjectSymbol? FindFirst(ObjectIdentifier objectIdentifier, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public IEnumerable<ObjectSymbol> FindAll(ObjectReference objectReference, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
    }

    public interface IObjectSymbolsCollection<T> where T : ObjectSymbol
    {
        public IEnumerable<T> Filter(AccessLevelFilter accessLevelFilter);
        public T? FindFirst(int id, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public T? FindFirst(ObjectReference reference, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public T? FindFirst(ObjectIdentifier objectIdentifier, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
        public IEnumerable<T> FindAll(ObjectReference objectReference, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible);
    }

}
