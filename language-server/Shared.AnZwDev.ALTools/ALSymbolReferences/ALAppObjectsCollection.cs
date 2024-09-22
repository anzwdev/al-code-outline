using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Core;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppObjectsCollection<T> : ALAppSymbolsCollection<T>, IALAppObjectsCollection where T : ALAppObject
    {

        public ALBaseSymbolReference SymbolReference { get; }

        private readonly DictionaryWithDuplicates<int, T> _objectsById = new DictionaryWithDuplicates<int, T>();
        private readonly DictionaryWithDuplicates<string, T> _objectsByName = new DictionaryWithDuplicates<string, T>(StringComparer.OrdinalIgnoreCase);

        public ALAppObjectsCollection() : this(null)
        {
        }

        public ALAppObjectsCollection(ALBaseSymbolReference symbolReference)
        {
            SymbolReference = symbolReference;
        }

        protected override void OnItemAdded(T item)
        {
            base.OnItemAdded(item);
            _objectsById.Add(item.Id, item);
            if (item.Name != null)
                _objectsByName.Add(item.Name, item);
            _usesNamespacesInitialized = false;
            if (SymbolReference != null)
                SymbolReference.OnObjectAdded(item);
        }

        protected override void OnItemRemoved(T item)
        {
            base.OnItemRemoved(item);
            _objectsById.Remove(item.Id, item);
            if (item.Name != null)
                _objectsByName.Remove(item.Name, item);
            _usesNamespacesInitialized = false;
            if (SymbolReference != null)
                SymbolReference.OnObjectRemoved(item);
        }

        protected override void OnClear()
        {
            base.OnClear();
            _objectsById.Clear();
            _objectsByName.Clear();
            _usesNamespacesInitialized = false;
            if (SymbolReference != null)
                SymbolReference.OnObjectsClear();
        }

        public T FindFirst(int id, bool includeInternal = true)
        {
            T obj = _objectsById.GetFirstValue(id);
            if ((obj != null) && (includeInternal || (!obj.IsInternal())))
                return obj;
            return null;
        }

        public T FindFirst(ALObjectReference reference, bool includeInternal = true)
        {
            if (reference.ObjectId != 0)
                return FindFirst(reference.ObjectId, includeInternal);

            if (_objectsByName.ContainsSingleElementKey(reference.Name))
            {                
                var obj = _objectsByName.GetSingleValue(reference.Name);
                if ((reference.MatchNamespace(obj.NamespaceName)) && (includeInternal || (!obj.IsInternal())))
                    return obj;
            }
            else if (_objectsByName.ContainsMultipleElementsKey(reference.Name))
            {
                var objList = _objectsByName.GetMultipleValues(reference.Name);
                for (int i=0; i<objList.Count; i++)
                    if ((reference.MatchNamespace(objList[i].NamespaceName)) && (includeInternal || (!objList[i].IsInternal())))
                        return objList[i];
            }

            return null;
        }

        public T FindFirst(ALObjectIdentifier objectIdentifier, bool includeInternal = true)
        {
            if (objectIdentifier.Id != 0)
                return FindFirst(objectIdentifier.Id, includeInternal);

            return FindFirst(objectIdentifier.NamespaceName, objectIdentifier.Name, includeInternal);
        }

        public T FindFirst(string namespaceName, string name, bool includeInternal = true)
        {
            bool checkNamespace = !String.IsNullOrWhiteSpace(namespaceName);

            if (_objectsByName.ContainsSingleElementKey(name))
            {
                var obj = _objectsByName.GetSingleValue(name);
                if (
                        ((!checkNamespace) || (namespaceName.Equals(obj.NamespaceName, StringComparison.OrdinalIgnoreCase))) && 
                        (includeInternal || (!obj.IsInternal()))
                    )
                    return obj;
            }
            else if (_objectsByName.ContainsMultipleElementsKey(name))
            {
                var objList = _objectsByName.GetMultipleValues(name);
                for (int i = 0; i < objList.Count; i++)
                    if (
                            ((!checkNamespace) || (namespaceName.Equals(objList[i].NamespaceName, StringComparison.OrdinalIgnoreCase))) &&
                            (includeInternal || (!objList[i].IsInternal()))
                        )
                        return objList[i];
            }

            return null;
        }

        public IEnumerable<T> FindAll(ALObjectReference reference, bool includeInternal = true)
        {
            if (reference.ObjectId != 0)
            {
                if (_objectsById.ContainsSingleElementKey(reference.ObjectId))
                {
                    var obj = _objectsById.GetSingleValue(reference.ObjectId);
                    if (includeInternal || (!obj.IsInternal()))
                        yield return obj;
                } 
                else if (_objectsById.ContainsMultipleElementsKey(reference.ObjectId))
                {
                    var objList = _objectsById.GetMultipleValues(reference.ObjectId);
                    for (int i = 0; i < objList.Count; i++)
                        if (includeInternal || (!objList[i].IsInternal()))
                            yield return objList[i];
                }
            }
            else if (_objectsByName.ContainsSingleElementKey(reference.Name))
            {
                var obj = _objectsByName.GetSingleValue(reference.Name);
                if ((reference.MatchNamespace(obj.NamespaceName)) && (includeInternal || (!obj.IsInternal())))
                    yield return obj;
            }
            else if (_objectsByName.ContainsMultipleElementsKey(reference.Name))
            {
                var objList = _objectsByName.GetMultipleValues(reference.Name);
                for (int i = 0; i < objList.Count; i++)
                    if ((reference.MatchNamespace(objList[i].NamespaceName)) && (includeInternal || (!objList[i].IsInternal())))
                        yield return objList[i];
            }
        }

        public IEnumerable<long> GetIdsEnumerable()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this[i].Id;
            }
        }

        public void Replace(T element)
        {
            T existingItem = FindFirst(element.Id);
            if (existingItem != null)
                Remove(existingItem);
            Add(element);
        }

        private bool _usesNamespaces = false;
        private bool _usesNamespacesInitialized = false;
        private void InitializeUsesNamespaces()
        {
            if (!_usesNamespacesInitialized)
            {
                _usesNamespacesInitialized = true;
                for (int i = 0; i < this.Count; i++)
                    if (!String.IsNullOrWhiteSpace(this[i].NamespaceName))
                    {
                        _usesNamespaces = true;
                        return;
                    }
                _usesNamespaces = false;
            }
        }

        public bool UsesNamespaces()
        {
            InitializeUsesNamespaces();
            return _usesNamespaces;
        }

        #region IALAppObjectsCollection implementation

        int ICollection<ALAppObject>.Count => Count;

        bool ICollection<ALAppObject>.IsReadOnly => false;

        ALAppObject IList<ALAppObject>.this[int index] { get => this[index]; set => this[index] = (T)value; }

        void IALAppObjectsCollection.Replace(ALAppObject alAppObject)
        {
            Replace((T)alAppObject);
        }

        void ICollection<ALAppObject>.Add(ALAppObject item)
        {
            Add((T)item);
        }

        void ICollection<ALAppObject>.Clear()
        {
            Clear();
        }

        bool ICollection<ALAppObject>.Contains(ALAppObject item)
        {
            return Contains((T)item);
        }

        void ICollection<ALAppObject>.CopyTo(ALAppObject[] array, int arrayIndex)
        {
        }

        bool ICollection<ALAppObject>.Remove(ALAppObject item)
        {
            return Remove((T)item);
        }

        IEnumerator<ALAppObject> IEnumerable<ALAppObject>.GetEnumerator()
        {
            return GetEnumerator();
        }

        int IList<ALAppObject>.IndexOf(ALAppObject item)
        {
            return IndexOf((T)item);
        }

        void IList<ALAppObject>.Insert(int index, ALAppObject item)
        {
            Insert(index, (T)item);
        }

        void IList<ALAppObject>.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        ALAppObject IALAppObjectsCollection.FindFirst(string namespaceName, string name)
        {
            return FindFirst(namespaceName, name);
        }

        ALAppObject IALAppObjectsCollection.FindFirst(ALObjectReference objectReference)
        {
            return FindFirst(objectReference);
        }

        ALAppObject IALAppObjectsCollection.FindFirst(int id)
        {
            return FindFirst(id);
        }

        ALAppObject IALAppObjectsCollection.FindFirst(int id, bool includeInternal)
        {
            return FindFirst(id, includeInternal);
        }

        #endregion

    }
}
