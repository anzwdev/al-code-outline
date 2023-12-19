using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.ALSymbolReferences.MergedReferences
{
    // !!! TO-DO !!!
    // !!! Clean file !!!


    /*

    public class MergedProjectALAppObjectsContainer<T> where T : ALAppObject
    {

        protected ALProject Project { get; }
        protected Func<ALAppSymbolReference, IList<T>> GetALAppObjectsCollection { get; }

        public MergedProjectALAppObjectsContainer(ALProject project, Func<ALAppSymbolReference, IList<T>> getALAppObjectsCollection)
        {
            this.Project = project;
            this.GetALAppObjectsCollection = getALAppObjectsCollection;
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

        public IEnumerator<T> GetEnumerator()
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    */
}
