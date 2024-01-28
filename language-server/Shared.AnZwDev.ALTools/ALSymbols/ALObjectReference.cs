using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols
{
    public struct ALObjectReference
    {

        public HashSet<string> Usings { get; }
        public string NamespaceName { get; }
        public string Name { get; }
        public int ObjectId { get; }
        public string NameWithNamespace { get; }

        public ALObjectReference(HashSet<string> usings, string nameWithNamespaceOrId)
        {
            Usings = usings;
            NamespaceName = null;
            Name = null;
            ObjectId = 0;
            NameWithNamespace = null;

            if (!String.IsNullOrWhiteSpace(nameWithNamespaceOrId))
            {
                if (Int32.TryParse(nameWithNamespaceOrId, out int objectId))
                    ObjectId = objectId;
                else
                {
                    NameWithNamespace = nameWithNamespaceOrId;
                    var namespaceEndPos = FindNamespaceEndPos(nameWithNamespaceOrId);
                    if (namespaceEndPos >= 0)
                    {
                        NamespaceName = nameWithNamespaceOrId.Substring(0, namespaceEndPos);
                        Name = ALSyntaxHelper.DecodeName(nameWithNamespaceOrId.Substring(namespaceEndPos + 1));
                    }
                    else
                        Name = ALSyntaxHelper.DecodeName(nameWithNamespaceOrId);
                }
            }
        }

        public ALObjectReference(HashSet<string> usings, string namespaceName, int id, string name)
        {
            Usings = usings;
            NamespaceName = namespaceName;
            ObjectId = id;
            Name = name;
            NameWithNamespace = null;

            if (String.IsNullOrWhiteSpace(name))
                NameWithNamespace = null;
            else
            {
                var fullName = ALSyntaxHelper.EncodeName(Name);
                if (HasNamespace())
                    NameWithNamespace = NamespaceName + "." + fullName;
                else
                    NameWithNamespace = fullName;
            }
        }

        public ALObjectReference(HashSet<string> usings, string namespaceName, string name) : this(usings, namespaceName, 0, name)
        { 
        }

        public bool HasNamespace()
        { 
            return !String.IsNullOrWhiteSpace(NamespaceName);
        }

        public bool HasUsings()
        {
            return (Usings != null) && (Usings.Count > 0);
        }

        public bool IsEmpty()
        {
            return (ObjectId == 0) && (String.IsNullOrWhiteSpace(Name));
        }

        public string GetFullName()
        {
            var fullName = ALSyntaxHelper.EncodeName(Name);
            if (HasNamespace())
                return NamespaceName + "." + fullName;
            return fullName;
        }

        private int FindNamespaceEndPos(string nameWithNamespace)
        {
            bool inName = false;
            for (int i = nameWithNamespace.Length - 1; i >= 0; i--)
            {
                switch (nameWithNamespace[i])
                {
                    case '"':
                        inName = !inName;
                        break;
                    case '.':
                        if (!inName)
                            return i;
                        break;
                }
            }
            return -1;
        }

    }
}
