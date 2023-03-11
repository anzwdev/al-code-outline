using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class ObjectInformationProvider
    {

        public List<ObjectInformation> GetProjectObjects(ALProject project, HashSet<ALSymbolKind> includeObjects, bool includeDependencies, bool includeObsolete)
        {
            List<ObjectInformation> objectInformationCollection = new List<ObjectInformation>();
            this.AddObjects(project.Symbols, includeObjects, objectInformationCollection, includeObsolete);
            if ((includeDependencies) && (project.Dependencies != null))
            {
                for (int i=0; i<project.Dependencies.Count; i++)
                {
                    this.AddObjects(project.Dependencies[i].Symbols, includeObjects, objectInformationCollection, includeObsolete);
                }
            }    
            return objectInformationCollection;
        }

        protected void AddObjects(ALAppSymbolReference appSymbolReference, HashSet<ALSymbolKind> includeObjects, List<ObjectInformation> objectInformationCollection, bool includeObsolete)
        {
            if (appSymbolReference != null)
            {
                IEnumerable<ALAppObject> alAppObjectsEnumerable = appSymbolReference.GetAllALAppObjectsEnumerable(includeObjects);
                foreach (ALAppObject alAppObject in alAppObjectsEnumerable)
                {
                    bool validObject = true;
                    if (!includeObsolete)
                    {
                        string obsoleteState = alAppObject?.Properties?.GetValue("ObsoleteState");
                        validObject = ((String.IsNullOrWhiteSpace(obsoleteState)) || (!obsoleteState.Equals("Removed", StringComparison.CurrentCultureIgnoreCase)));
                    }
                    if (validObject)
                        objectInformationCollection.Add(new ObjectInformation(alAppObject));
                }
            }
        }

    }
}
