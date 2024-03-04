using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.Workspace
{
    public class ALProjectAllALAppObjectsCollection<T> : IALProjectAllALAppObjectsCollection where T : ALAppObject
    {

        public ALObjectType ObjectType { get; }
        public ALProject Project { get; }
        private readonly Func<ALAppSymbolReference, ALAppObjectsCollection<T>> _objectsCollection;

        public ALProjectAllALAppObjectsCollection(ALProject project, ALObjectType objectType, Func<ALAppSymbolReference, ALAppObjectsCollection<T>> objectsCollection)
        {
            Project = project;
            ObjectType = objectType;
            _objectsCollection = objectsCollection;
        }

        public T FindFirst(int id, bool includeInaccessible = false)
        {
            var obj = _objectsCollection(Project.Symbols)?.FindFirst(id);
            if (obj != null)
                return obj;

            for (int i=0; i<Project.Dependencies.Count; i++)
            {
                var includeInternal = includeInaccessible || Project.Dependencies[i].InternalsVisible;
                obj = _objectsCollection(Project.Dependencies[i].Symbols)?.FindFirst(id, includeInternal);
                if (obj != null)
                    return obj;
            }

            return null;
        }

        public T FindFirst(ALObjectReference reference, bool includeInaccessible = false)
        {
            var obj = _objectsCollection(Project.Symbols)?.FindFirst(reference);
            if (obj != null)
                return obj;

            for (int i=0; i<Project.Dependencies.Count; i++)
            {
                var includeInternal = includeInaccessible || Project.Dependencies[i].InternalsVisible;
                obj = _objectsCollection(Project.Dependencies[i].Symbols)?.FindFirst(reference, includeInternal);
                if (obj != null)
                    return obj;
            }

            return null;
        }

        public T FindFirst(ALObjectIdentifier identifier, bool includeInaccessible = false)
        {
            var obj = _objectsCollection(Project.Symbols)?.FindFirst(identifier);
            if (obj != null)
                return obj;

            for (int i = 0; i < Project.Dependencies.Count; i++)
            {
                var includeInternal = includeInaccessible || Project.Dependencies[i].InternalsVisible;
                obj = _objectsCollection(Project.Dependencies[i].Symbols)?.FindFirst(identifier, includeInternal);
                if (obj != null)
                    return obj;
            }

            return null;
        }


        public T FindFirst(string namespaceName, string name, bool includeInaccessible = false)
        {
            var obj = _objectsCollection(Project.Symbols)?.FindFirst(namespaceName, name);
            if (obj != null)
                return obj;

            for (int i=0; i<Project.Dependencies.Count; i++)
            {
                var includeInternal = includeInaccessible || Project.Dependencies[i].InternalsVisible;
                obj = _objectsCollection(Project.Dependencies[i].Symbols)?.FindFirst(namespaceName, name, includeInternal);
                if (obj != null)
                    return obj;
            }

            return null;
        }

        public IEnumerable<T> FindAll(ALObjectReference reference, bool includeInaccessible = false)
        {
            var objectEnumerable = _objectsCollection(Project.Symbols)?.FindAll(reference);
            if (objectEnumerable != null)
                foreach (var obj in objectEnumerable)
                    yield return obj;

            for (int i = 0; i < Project.Dependencies.Count; i++)
            {
                var includeInternal = includeInaccessible || Project.Dependencies[i].InternalsVisible;
                objectEnumerable = _objectsCollection(Project.Dependencies[i].Symbols)?.FindAll(reference, includeInternal);
                if (objectEnumerable != null)
                    foreach (var obj in objectEnumerable)
                        yield return obj;
            }
        }

        public IEnumerable<T> GetAll(bool includeInaccessible = false)
        {
            var objectEnumerable = _objectsCollection(Project.Symbols);
            if (objectEnumerable != null)
                foreach (var obj in objectEnumerable)
                    yield return obj;

            for (int i = 0; i < Project.Dependencies.Count; i++)
            {
                var includeInternal = includeInaccessible || Project.Dependencies[i].InternalsVisible;
                objectEnumerable = _objectsCollection(Project.Dependencies[i].Symbols);
                if (objectEnumerable != null)
                    foreach (var obj in objectEnumerable)
                        if (includeInternal || (!obj.IsInternal()))
                            yield return obj;
            }
        }

        public IEnumerable<T> Filter(HashSet<string> dependenciesFilter, bool includeInaccessible = false)
        {

            var objectEnumerable = _objectsCollection(Project.Symbols);
            if (objectEnumerable != null)
                foreach (var obj in objectEnumerable)
                    yield return obj;

            for (int i = 0; i < Project.Dependencies.Count; i++)
            {
                if ((dependenciesFilter == null) || (dependenciesFilter.Contains(Project.Dependencies[i].Symbols.GetNameWithPublisher())))
                {
                    var includeInternal = includeInaccessible || Project.Dependencies[i].InternalsVisible;
                    objectEnumerable = _objectsCollection(Project.Dependencies[i].Symbols);
                    if (objectEnumerable != null)
                        foreach (var obj in objectEnumerable)
                            if (includeInternal || (!obj.IsInternal()))
                                yield return obj;
                }
            }
        }

        IEnumerable<ALAppObject> IALProjectAllALAppObjectsCollection.GetAll(bool includeInaccessible)
        {
            return this.GetAll(includeInaccessible);
        }

        ALAppObject IALProjectAllALAppObjectsCollection.FindFirst(ALObjectReference reference, bool includeInaccessible)
        {
            return FindFirst(reference, includeInaccessible);
        }
    }
}
