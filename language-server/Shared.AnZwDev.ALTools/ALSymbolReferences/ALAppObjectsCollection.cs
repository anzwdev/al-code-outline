using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using AnZwDev.ALTools.ALSymbolReferences.Search;

namespace AnZwDev.ALTools.ALSymbolReferences
{
    public class ALAppObjectsCollection<T> : ALAppSymbolsCollection<T>, IALAppObjectsCollection where T : ALAppObject
    {

        public ALAppObjectsCollection()
        {
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
            T existingItem = ((IEnumerable<T>)this).FindFirst(element.Id);
            if (existingItem != null)
                Remove(existingItem);
            Add(element);
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

        #endregion

    }
}
