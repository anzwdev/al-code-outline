using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbolReferences.Search;
using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolReferences
{
    public static class ALProjectSymbolReferencesExtensions
    {

        public static IEnumerable<ALProjectSymbolReferenceWithAccessPermissions> GetAllSymbolReferences(this ALProject project)
        {
            if (project.Symbols != null)
                yield return new ALProjectSymbolReferenceWithAccessPermissions(project.Symbols, true, true);
            foreach (var dependency in project.Dependencies)
                if (dependency.Symbols != null)
                    yield return new ALProjectSymbolReferenceWithAccessPermissions(dependency.Symbols, dependency.InternalsVisible, false);
        }

        public static IEnumerable<ALProjectSymbolReferenceWithAccessPermissions> FilterByName(this IEnumerable<ALProjectSymbolReferenceWithAccessPermissions> symbolReferencesEnumerable, HashSet<string> dependencyNames)
        {
            foreach (var symbolReferences in symbolReferencesEnumerable)
                if ((dependencyNames == null) || (dependencyNames.Contains(symbolReferences.Symbols.GetNameWithPublisher())))
                    yield return symbolReferences;
        }

        public static IEnumerable<T> GetAllObjects<T>(this IEnumerable<ALProjectSymbolReferenceWithAccessPermissions> symbolReferences, Func<ALAppSymbolReference, IEnumerable<T>> objectsEnumerable, bool includeNonAccessible = false) where T : ALAppObject
        {
            foreach (var symbolReference in symbolReferences)
            {
                var objects = objectsEnumerable(symbolReference.Symbols);
                if (objects != null)
                    foreach (var obj in objects)
                        if ((includeNonAccessible) || (symbolReference.InternalsVisible) || (!obj.IsInternal()))
                            yield return obj;
            }
        }

        public static IEnumerable<ALAppObject> GetAllObjects(this IEnumerable<ALProjectSymbolReferenceWithAccessPermissions> symbolReferences, ALObjectType objectType, bool includeNonAccessible = false)
        {
            foreach (var symbolReference in symbolReferences)
            {
                var objects = symbolReference.Symbols?.AllObjects?.FilterByObjectType(objectType);
                if (objects != null)
                    foreach (var obj in objects)
                        if ((includeNonAccessible) || (symbolReference.InternalsVisible) || (!obj.IsInternal()))
                            yield return obj;
            }
        }

        public static IEnumerable<ALAppObject> GetAllObjects(this IEnumerable<ALProjectSymbolReferenceWithAccessPermissions> symbolReferences, HashSet<ALObjectType> objectType, bool includeNonAccessible = false)
        {
            foreach (var symbolReference in symbolReferences)
            {
                var objects = symbolReference.Symbols?.AllObjects?.FilterByObjectType(objectType);
                if (objects != null)
                    foreach (var obj in objects)
                        if ((includeNonAccessible) || (symbolReference.InternalsVisible) || (!obj.IsInternal()))
                            yield return obj;
            }
        }

        public static IEnumerable<T> GetObjectExtensions<T>(this IEnumerable<ALProjectSymbolReferenceWithAccessPermissions> symbolReferences, Func<ALAppSymbolReference, IEnumerable<T>> objectsEnumerable, ALObjectIdentifier extendedObjectIdentifier, bool includeNonAccessible = false) where T : ALAppObject, IALAppObjectExtension
        {
            foreach (var symbolReference in symbolReferences)
            {
                var objects = objectsEnumerable(symbolReference.Symbols);
                if (objects != null)
                {
                    var extensionObject = objects.FindFirstExtension(extendedObjectIdentifier);
                    if ((extensionObject != null) && ((includeNonAccessible) || (symbolReference.InternalsVisible) || (!extensionObject.IsInternal())))
                        yield return extensionObject;
                }
            }
        }

    }
}
