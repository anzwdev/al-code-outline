using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.MergedReferences
{
    public class MergedALAppObjectExtensionsCollection<T> : MergedALAppObjectsCollection<T> where T : ALAppObject, IALAppObjectExtension
    {

        public MergedALAppObjectExtensionsCollection(ISymbolReferencesList allSymbols, ALSymbolKind alSymbolKind, Func<ALAppSymbolReference, IList<T>> getALAppObjectsCollection) : base(allSymbols, alSymbolKind, getALAppObjectsCollection)
        {
        }

        protected virtual bool ExtendsObject(T alObject, string baseObjectName)
        {
            var targetName = alObject.GetTargetObjectName();
            return ((targetName != null) && (targetName.Equals(baseObjectName, StringComparison.CurrentCultureIgnoreCase)));
        }

        public IEnumerable<T> FindAllExtensions(string baseObjectName)
        {
            for (int objListIdx = 0; objListIdx < this.AllSymbolReferences.Count; objListIdx++)
            {
                IList<T> objectsList = this.GetALAppObjectsCollection(this.AllSymbolReferences[objListIdx]);
                if (objectsList != null)
                {
                    T objectExtension = this.FindFirstExtension(objectsList, baseObjectName);
                    if (objectExtension != null)
                        yield return objectExtension;
                }
            }
        }

        private T FindFirstExtension(IList<T> objectsList, string baseObjectName)
        {
            for (int objIdx = 0; objIdx < objectsList.Count; objIdx++)
                if ((objectsList[objIdx] != null) && (this.ExtendsObject(objectsList[objIdx], baseObjectName)))
                    return objectsList[objIdx];
            return null;
        }

    }
}
