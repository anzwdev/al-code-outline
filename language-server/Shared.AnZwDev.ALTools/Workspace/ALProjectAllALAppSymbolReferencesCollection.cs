using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.MergedReferences;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace
{

    public class ALProjectAllALAppSymbolReferencesCollection : ISymbolReferencesList
    {

        protected ALProject ALProject { get; }

        public int Count => this.ALProject.Dependencies.Count + 1;

        public ALAppSymbolReference this[int index] 
        { 
            get
            {
                if (index == 0)
                    return this.ALProject.Symbols;
                return this.ALProject.Dependencies[index - 1].Symbols;
            }
            set => throw new NotImplementedException(); 
        }

        public bool InternalsVisible(int index)
        {
            if (index == 0)
                return true;
            return this.ALProject.Dependencies[index - 1].InternalsVisible;
        }

        public ALProjectAllALAppSymbolReferencesCollection(ALProject newProject)
        {
            this.ALProject = newProject;
        }

        public IEnumerator<ALAppSymbolReference> GetEnumerator()
        {
            yield return this.ALProject.Symbols;
            for (int i=0; i<this.ALProject.Dependencies.Count; i++)
            {
                yield return this.ALProject.Dependencies[i].Symbols;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
