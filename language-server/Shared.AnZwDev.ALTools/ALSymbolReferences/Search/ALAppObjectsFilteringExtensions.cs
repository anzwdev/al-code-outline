using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.ALTools.ALSymbolReferences.Search
{
    public static class ALAppObjectsFilteringExtensions
    {

        public static T FindFirst<T>(this IEnumerable<T> objects, ALObjectIdentifier objectIdentifier) where T : ALAppObject
        {
            return FindFirst<T>(objects, objectIdentifier.Id);
        }

        public static T FindFirst<T>(this IEnumerable<T> objects, ALObjectReference objectReference) where T : ALAppObject
        {
            if (objectReference.ObjectId != 0)
                return FindFirst<T>(objects, objectReference.ObjectId);
            var found = FindFirst<T>(objects, objectReference.Usings, objectReference.NamespaceName, objectReference.Name);

            if ((found == null) && (!String.IsNullOrWhiteSpace(objectReference.NamespaceName)))
                found = FindFirst<T>(objects, objectReference.Usings, null, objectReference.NameWithNamespace);

            return found;
        }

        public static T FindFirst<T>(this IEnumerable<T> objects, int id) where T : ALAppObject
        {
            if (objects != null)
                foreach (var item in objects)
                    if (item.Id == id)
                        return item;
            return null;
        }

        public static T FindFirst<T>(this IEnumerable<T> objects, HashSet<string> usingsNamespacesNames, string namespaceName, string name) where T : ALAppObject
        {
            if ((objects == null) || (String.IsNullOrWhiteSpace(name)))
                return null;

            bool hasNamespaceName = !String.IsNullOrWhiteSpace(namespaceName);
            bool hasUsings = (usingsNamespacesNames != null) && (usingsNamespacesNames.Count > 0);

            foreach (var item in objects)
                if
                    (
                        (name.Equals(item.Name, StringComparison.CurrentCultureIgnoreCase)) &&
                        (
                            ((!hasNamespaceName) && (!hasUsings)) ||
                            (
                                (hasNamespaceName) &&
                                (namespaceName.Equals(item.NamespaceName))
                            ) || (
                                (hasUsings) &&
                                (item.NamespaceName != null) &&
                                (usingsNamespacesNames.Contains(item.NamespaceName))
                            )
                        )
                    )
                    return item;

            return null;
        }

        public static T FindFirstExtension<T>(this IEnumerable<T> objects, ALObjectIdentifier extendedObjectIdentifier) where T : ALAppObject, IALAppObjectExtension
        {
            if (objects == null)
                return null;

            foreach (var item in objects)
                if (extendedObjectIdentifier.IsReferencedBy(item.GetTargetObjectReference()))
                    return item;

            return null;
        }

        public static IEnumerable<T> FindAll<T>(this IEnumerable<T> objects, ALObjectReference objectReference) where T : ALAppObject
        {
            if (objectReference.ObjectId != 0)
                return FindAll<T>(objects, objectReference.ObjectId);
            return FindAll<T>(objects, objectReference.Usings, objectReference.NamespaceName, objectReference.Name);
        }

        public static IEnumerable<T> FindAll<T>(this IEnumerable<T> objects, int id) where T : ALAppObject
        {
            if (objects != null)
                foreach (var item in objects)
                    if (item.Id == id)
                        yield return item;
        }

        public static IEnumerable<T> FindAll<T>(this IEnumerable<T> objects, HashSet<string> usingsNamespacesNames, string namespaceName, string name) where T : ALAppObject
        {
            if ((objects != null) && (!String.IsNullOrWhiteSpace(name)))
            {
                bool hasNamespaceName = !String.IsNullOrWhiteSpace(namespaceName);
                bool hasUsings = (usingsNamespacesNames != null) && (usingsNamespacesNames.Count > 0);

                foreach (var item in objects)
                    if
                        (
                            (name.Equals(item.Name, StringComparison.CurrentCultureIgnoreCase)) &&
                            (
                                ((!hasNamespaceName) && (!hasUsings)) ||
                                (
                                    (hasNamespaceName) &&
                                    (namespaceName.Equals(item.NamespaceName))
                                ) || (
                                    (hasUsings) &&
                                    (item.NamespaceName != null) &&
                                    (usingsNamespacesNames.Contains(item.NamespaceName))
                                )
                            )
                        )
                        yield return item;
            }
        }

        public static IEnumerable<(ALObjectIdentifier, T)> FindAllExtensions<T>(this IEnumerable<T> objects, IEnumerable<ALObjectIdentifier> extendedObjectIdentifiers) where T : ALAppObject, IALAppObjectExtension
        {
            if (objects != null)
            {
                foreach (var item in objects)
                {
                    var itemTargetReference = item.GetTargetObjectReference();
                    if (extendedObjectIdentifiers.Any(x => x.IsReferencedBy(itemTargetReference)))
                        yield return (extendedObjectIdentifiers.First(x => x.IsReferencedBy(itemTargetReference)), item);
                }
            }
        }

    }
}
