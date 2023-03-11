using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.MergedReferences
{
    public class MergedALAppObjectsCollection<T> : IReadOnlyALAppObjectsCollection where T : ALAppObject
    {

        public ALSymbolKind ALSymbolKind { get; }
        //protected IReadOnlyList<ALAppSymbolReference> AllSymbolReferences { get; }
        protected ISymbolReferencesList AllSymbolReferences { get; }
        protected Func<ALAppSymbolReference, IList<T>> GetALAppObjectsCollection { get; }


        public MergedALAppObjectsCollection(ISymbolReferencesList allSymbolReferences, ALSymbolKind aLSymbolKind, Func<ALAppSymbolReference, IList<T>> getALAppObjectsCollection)
        {
            this.ALSymbolKind = aLSymbolKind;
            this.AllSymbolReferences = allSymbolReferences;
            this.GetALAppObjectsCollection = getALAppObjectsCollection;
        }

        public IEnumerable<T> GetObjects()
        {
            return this.GetObjects(null);
        }

        public IEnumerable<T> GetObjects(HashSet<string> dependenciesNames, bool includeInternals = false)
        {
            for (int objListIdx = 0; objListIdx < this.AllSymbolReferences.Count; objListIdx++)
            {
                if ((dependenciesNames == null) || (dependenciesNames.Contains(this.AllSymbolReferences[objListIdx].GetNameWithPublisher())))
                {
                    var internalsVisible = AllSymbolReferences.InternalsVisible(objListIdx);

                    IList<T> objectsList = this.GetALAppObjectsCollection(this.AllSymbolReferences[objListIdx]);
                    if (objectsList != null)
                        for (int objIdx = 0; objIdx < objectsList.Count; objIdx++)
                            if ((objectsList[objIdx] != null) && (internalsVisible || (!objectsList[objIdx].IsInternal())))
                                yield return objectsList[objIdx];
                }
            }
        }

        public IEnumerable<long> GetIds()
        {
            for (int objListIdx = 0; objListIdx < this.AllSymbolReferences.Count; objListIdx++)
            {
                IList<T> objectsList = this.GetALAppObjectsCollection(this.AllSymbolReferences[objListIdx]);
                if (objectsList != null)
                    for (int objIdx = 0; objIdx < objectsList.Count; objIdx++)
                        if (objectsList[objIdx] != null)
                            yield return objectsList[objIdx].Id;
            }
        }

        public T FindObject(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            for (int objListIdx = 0; objListIdx < this.AllSymbolReferences.Count; objListIdx++)
            {
                IList<T> objectsList = this.GetALAppObjectsCollection(this.AllSymbolReferences[objListIdx]);
                if (objectsList != null)
                    for (int objIdx = 0; objIdx < objectsList.Count; objIdx++)
                        if ((objectsList[objIdx] != null) && (name.Equals(objectsList[objIdx].Name, StringComparison.CurrentCultureIgnoreCase)))
                            return objectsList[objIdx];
            }
            return null;
        }

        public T FindObject(int id)
        {
            for (int objListIdx = 0; objListIdx < this.AllSymbolReferences.Count; objListIdx++)
            {
                IList<T> objectsList = this.GetALAppObjectsCollection(this.AllSymbolReferences[objListIdx]);
                if (objectsList != null)
                    for (int objIdx = 0; objIdx < objectsList.Count; objIdx++)
                        if ((objectsList[objIdx] != null) && (objectsList[objIdx].Id == id))
                            return objectsList[objIdx];
            }
            return null;
        }

        public T FindObject(T item)
        {
            if (item == null)
                return null;

            for (int objListIdx = 0; objListIdx < this.AllSymbolReferences.Count; objListIdx++)
            {
                IList<T> objectsList = this.GetALAppObjectsCollection(this.AllSymbolReferences[objListIdx]);
                if (objectsList != null)
                    for (int objIdx = 0; objIdx < objectsList.Count; objIdx++)
                        if ((objectsList[objIdx] != null) && (objectsList[objIdx].Id == item.Id))
                            return objectsList[objIdx];
            }

            return null;
        }

        IEnumerable<ALAppObject> IReadOnlyALAppObjectsCollection.GetObjects()
        {
            return this.GetObjects();
        }
    }
}
