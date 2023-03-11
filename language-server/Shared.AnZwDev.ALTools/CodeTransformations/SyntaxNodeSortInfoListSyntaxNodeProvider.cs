using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SyntaxNodeSortInfoListSyntaxNodeProvider<T> : IList<T> where T : SyntaxNode
    {

        public List<SyntaxNodeSortInfo<T>> SyntaxNodeSortInfoList { get; }

        public int Count => this.SyntaxNodeSortInfoList.Count;

        public bool IsReadOnly => false;

        public T this[int index] 
        {
            get { return this.SyntaxNodeSortInfoList[index].Node; }
            set { this.SyntaxNodeSortInfoList[index].Node = value; }
        }

        public SyntaxNodeSortInfoListSyntaxNodeProvider(List<SyntaxNodeSortInfo<T>> list)
        {
            this.SyntaxNodeSortInfoList = list;
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
