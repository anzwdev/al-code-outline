using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AnZwDev.ALTools.ALSymbols
{
    public struct ALObjectIdentifier
    {

        public string NamespaceName { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public ALObjectIdentifier(string namespaceName, int id, string name)
        {
            NamespaceName = namespaceName;
            Id = id;
            Name = name;
        }

        public ALObjectIdentifier(string namespaceName, string id, string name) : this(namespaceName, id.ToInt(), name)
        {
        }

        public bool HasNamespace()
        {
            return !String.IsNullOrWhiteSpace(NamespaceName);
        }

        public bool IsEmpty()
        {
            return (Id == 0);
        }

        public bool IsReferencedBy(ALObjectReference objectReference)
        {
            if (objectReference.IsEmpty())
                return false;

            if ((Id != 0) && (Id == objectReference.ObjectId))
                return true;

            if ((!String.IsNullOrEmpty(Name)) && (Name.Equals(objectReference.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
                if ((!objectReference.HasNamespace()) && (!objectReference.HasUsings()))
                    return true;

                if (!HasNamespace())
                    return false;

                if (NamespaceName.Equals(objectReference.NamespaceName, StringComparison.CurrentCultureIgnoreCase))
                    return true;

                if (objectReference.Usings != null)
                    foreach (var usingNamespace in objectReference.Usings)
                        if (NamespaceName.Equals(usingNamespace, StringComparison.CurrentCultureIgnoreCase))
                            return true;
            }

            return false;
        }

        public ALObjectReference ToObjectReference()
        {
            return new ALObjectReference(null, NamespaceName, Id, Name);
        }

    }
}
