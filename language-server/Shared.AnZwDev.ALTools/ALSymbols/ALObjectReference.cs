using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AnZwDev.ALTools.ALSymbols
{
    public struct ALObjectReference
    {

        public HashSet<string> Usings { get; set; }
        public string NamespaceName { get; set; }
        public string Name { get; set; }
        public int ObjectId { get; set; }

        public ALObjectReference(HashSet<string> usings, string nameWithNamespaceOrId)
        {
            Usings = usings;
            NamespaceName = null;
            Name = null;
            ObjectId = 0;

            if (!String.IsNullOrWhiteSpace(nameWithNamespaceOrId))
            {
                if (Int32.TryParse(nameWithNamespaceOrId, out int objectId))
                    ObjectId = objectId;
                else
                {
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
