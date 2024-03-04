using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectAllALAppObjectExtensionsCollection<T> : ALProjectAllALAppObjectsCollection<T> where T : ALAppObject, IALAppObjectExtension
    {

        private readonly Func<ALAppSymbolReference, ALAppObjectExtensionsCollection<T>> _objectExtensionsCollection;

        public ALProjectAllALAppObjectExtensionsCollection(ALProject project, ALObjectType objectType, Func<ALAppSymbolReference, ALAppObjectExtensionsCollection<T>> objectsCollection)
            : base(project, objectType, objectsCollection)
        {
            _objectExtensionsCollection = objectsCollection;
        }

        public IEnumerable<T> GetObjectExtensions(ALObjectIdentifier extendedObjectIdentifier, HashSet<string> dependenciesFilter = null, bool includeInaccessible = false)
        {
            var objectExtension = _objectExtensionsCollection(Project.Symbols)?.FindFirstObjectExtension(extendedObjectIdentifier);
            if (objectExtension != null)
                yield return objectExtension;

            for (int depIdx = 0; depIdx < Project.Dependencies.Count; depIdx++)
            {
                if ((dependenciesFilter == null) || (dependenciesFilter.Contains(Project.Dependencies[depIdx].Symbols.GetNameWithPublisher())))
                {
                    var includeInternal = includeInaccessible || Project.Dependencies[depIdx].InternalsVisible;
                    objectExtension = _objectExtensionsCollection(Project.Dependencies[depIdx].Symbols)?.FindFirstObjectExtension(extendedObjectIdentifier, includeInternal);
                    if (objectExtension != null)
                        yield return objectExtension;
                }
            }
        }

        public IEnumerable<(ALObjectIdentifier, T)> GetObjectExtensions(List<ALObjectIdentifier> extendedObjectIdentifiersList, HashSet<string> dependenciesFilter = null, bool includeInaccessible = false)
        {
            for (int idIdx = 0; idIdx < extendedObjectIdentifiersList.Count; idIdx++)
            {
                var extendedObjectIdentifier = extendedObjectIdentifiersList[idIdx];

                //code copied from function above to avoid creation of multiple enumerators
                var objectExtension = _objectExtensionsCollection(Project.Symbols)?.FindFirstObjectExtension(extendedObjectIdentifier);
                if (objectExtension != null)
                    yield return (extendedObjectIdentifier, objectExtension);

                for (int depIdx = 0; depIdx < Project.Dependencies.Count; depIdx++)
                {
                    if ((dependenciesFilter == null) || (dependenciesFilter.Contains(Project.Dependencies[depIdx].Symbols.GetNameWithPublisher())))
                    {
                        var includeInternal = includeInaccessible || Project.Dependencies[depIdx].InternalsVisible;
                        objectExtension = _objectExtensionsCollection(Project.Dependencies[depIdx].Symbols)?.FindFirstObjectExtension(extendedObjectIdentifier, includeInternal);
                        if (objectExtension != null)
                            yield return (extendedObjectIdentifier, objectExtension);
                    }
                }
            }
        }

    }
}
