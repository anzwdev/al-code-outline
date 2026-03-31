using AnZwDev.System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Collections
{
    public partial class ObjectSymbolsCollection<T> : ExtendableList<T>, IObjectSymbolsCollection<T>, IObjectSymbolsCollection where T : ObjectSymbol
    {

        public bool Contains(T symbol, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            return Contains(symbol.Identifier, accessLevelFilter);
        }

        public bool Contains(ObjectIdentifier identifier, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            return (FindFirst(identifier, accessLevelFilter) != null);
        }

        public IEnumerable<T> Filter(AccessLevelFilter accessLevelFilter)
        {
            for (int i = 0; i < this.Count; i++)
                if (accessLevelFilter.Valid(this[i].AccessLevel))
                    yield return this[i];
        }

        public T? FindFirst(int id, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            var list = _objectsById.GetAll(id);
            if (list != null)
                for (int i = 0; i < list.Count; i++)
                    if (accessLevelFilter.Valid(list[i].AccessLevel))
                        return list[i];
            return null;
        }

        public T? FindFirst(ObjectReference reference, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            if (reference.ObjectId != 0)
                return FindFirst(reference.ObjectId, accessLevelFilter);

            if (!String.IsNullOrEmpty(reference.FullyQualifiedName.Name))
            {
                var list = _objectsByName.GetAll(reference.FullyQualifiedName.Name);
                if (list != null)
                    for (int i = 0; i < list.Count; i++)
                        if (
                            (accessLevelFilter.Valid(list[i].AccessLevel)) &&
                            (reference.ReferencesNamespace(list[i].Identifier.FullyQualifiedName.Namespace))
                            )
                            return list[i];
            }

            return null;
        }

        public T? FindFirst(ObjectIdentifier objectIdentifier, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            if (objectIdentifier.Id != 0)
                return FindFirst(objectIdentifier.Id, accessLevelFilter);

            var list = _objectsByName.GetAll(objectIdentifier.FullyQualifiedName.Name);
            if (list == null)
                return null;

            var namespaceName = objectIdentifier.FullyQualifiedName.Namespace;

            for (int i = 0; i < list.Count; i++)
                if (
                    (accessLevelFilter.Valid(list[i].AccessLevel)) &&
                    (list[i].Identifier.FullyQualifiedName.NamespaceEquals(namespaceName))
                    )
                    return list[i];

            return null;
        }

        public IEnumerable<T> FindAll(ObjectReference objectReference, AccessLevelFilter accessLevelFilter = AccessLevelFilter.Accessible)
        {
            List<T>? list = null;
            bool matchNamespace = (objectReference.ObjectId == 0);

            if (objectReference.ObjectId != 0)
                list = _objectsById.GetAll(objectReference.ObjectId);
            else if (!String.IsNullOrWhiteSpace(objectReference.FullyQualifiedName.Name))
                list = _objectsByName.GetAll(objectReference.FullyQualifiedName.Name);

            if (list != null)
                for (int i = 0; i < list.Count; i++)
                {
                    var symbol = list[i];
                    if (
                        (accessLevelFilter.Valid(symbol.AccessLevel)) &&
                        ((!matchNamespace) || (objectReference.ReferencesNamespace(symbol.Identifier.FullyQualifiedName.Namespace)))
                    )
                        yield return symbol;
                }
        }

    }
}
