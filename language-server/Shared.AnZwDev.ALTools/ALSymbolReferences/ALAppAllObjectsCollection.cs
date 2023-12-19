using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppAllObjectsCollection : ICollection<ALAppObject>
    {

        #region Public properties

        public int Count
        {
            get
            {
                var count = 0;
                for (int i = 0; i < _objectsCollections.AllCollections.Count; i++)
                    if (_objectsCollections.AllCollections[i].Objects != null)
                        count += _objectsCollections.AllCollections[i].Objects.Count;
                return count;
            }
        }

        public bool IsReadOnly => false;

        #endregion

        #region Private members

        private readonly ALAppObjectsCollectionsContainersCollection _objectsCollections;

        #endregion

        public ALAppAllObjectsCollection(ALAppObjectsCollectionsContainersCollection objectsCollections)
        {
            _objectsCollections = objectsCollections;
        }

        #region ICollection implementation

        public void AddRange(IEnumerable<ALAppObject> collection)
        {
            foreach (var item in collection)
                Add(item);
        }

        public void Add(ALAppObject item)
        {
            var collection = _objectsCollections.GetOrCreateCollection(item.GetALObjectType());
            if (collection != null)
                collection.Add(item);
        }

        public void RemoveRange(IEnumerable<ALAppObject> collection)
        {
            foreach (var item in collection)
                Remove(item);
        }

        public bool Remove(ALAppObject item)
        {
            var collection = _objectsCollections.GetOrCreateCollection(item.GetALObjectType());
            if (collection == null)
                return false;
            return collection.Remove(item);
        }

        public void Clear()
        {
            for (int i = 0; i < _objectsCollections.AllCollections.Count; i++)
                _objectsCollections.AllCollections[i].Objects?.Clear();
        }

        public bool Contains(ALAppObject item)
        {
            var collection = _objectsCollections.GetOrCreateCollection(item.GetALObjectType());
            if (collection == null)
                return false;
            return collection.Contains(item);
        }

        public void CopyTo(ALAppObject[] array, int arrayIndex)
        {
            for (int containerIndex = 0; containerIndex < _objectsCollections.AllCollections.Count; containerIndex++)
            {
                var objectsCollection = _objectsCollections.AllCollections[containerIndex].Objects;
                if (objectsCollection != null)
                    for (int objectIndex = 0; objectIndex < objectsCollection.Count; objectIndex++)
                    {
                        array[arrayIndex] = objectsCollection[objectIndex];
                        arrayIndex++;
                    }
            }
        }

        public IEnumerator<ALAppObject> GetEnumerator()
        {
            for (int containerIndex = 0; containerIndex < _objectsCollections.AllCollections.Count; containerIndex++)
            {
                var objectsCollection = _objectsCollections.AllCollections[containerIndex].Objects;
                if (objectsCollection != null)
                    for (int objectIndex = 0; objectIndex < objectsCollection.Count; objectIndex++)
                        yield return objectsCollection[objectIndex];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public IEnumerable<ALAppObject> FilterByObjectType(ALObjectType objectType)
        {
            return _objectsCollections.GetCollection(objectType);
        }

        public IEnumerable<ALAppObject> FilterByObjectType(HashSet<ALObjectType> objectTypes)
        {
            foreach (var objectType in objectTypes)
            {
                var collection = _objectsCollections.GetCollection(objectType);
                if (collection != null)
                    foreach (var item in collection)
                        yield return item;
            }
        }


    }
}
