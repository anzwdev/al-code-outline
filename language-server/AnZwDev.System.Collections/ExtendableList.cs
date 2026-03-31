using System.Collections;

namespace AnZwDev.System.Collections
{
    public class ExtendableList<T> : IList<T>, IEnumerable<T>, IEnumerable
    {

        private readonly List<T> _list = new List<T>();

        public T this[int index] { get => _list[index]; set => _list[index] = value; }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        private int _inUpdateLevel = 0;
        public bool IsInUpdate { get { return _inUpdateLevel > 0; } }

        public void AddRange(IList<T> items)
        {
            for (int i = 0; i < items.Count; i++)
                Add(items[i]);
        }

        public void Add(T item)
        {
            _list.Add(item);
            OnItemAdded(item);
        }

        public void Clear()
        {
            _list.Clear();
            OnClear();
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public List<T>.Enumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            OnItemAdded(item);
        }

        public bool Remove(T item)
        {
            var result = _list.Remove(item);
            if (result)
                OnItemRemoved(item);
            return result;
        }

        public void RemoveAt(int index)
        {
            var item = _list[index];
            _list.RemoveAt(index);
            OnItemRemoved(item);
        }

        protected virtual void OnItemAdded(T item)
        {
        }

        protected virtual void OnItemRemoved(T item)
        {
        }

        protected virtual void OnClear()
        {
        }

        public void BeginUpdate()
        {
            _inUpdateLevel++;
        }

        public void EndUpdate()
        {
            if (_inUpdateLevel > 0)
                _inUpdateLevel--;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
