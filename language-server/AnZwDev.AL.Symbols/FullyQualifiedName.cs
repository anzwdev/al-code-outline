using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols
{
    
    public struct FullyQualifiedName
    {

        public string? Namespace { get; init; }
        public string Name { get; init; }

        public FullyQualifiedName(string? ns, string? name)
        {
            Namespace = ns;
            Name = name ?? String.Empty;
        }

        public bool IsEmpty()
        {
            return String.IsNullOrWhiteSpace(Name);
        }

        public bool Equals(FullyQualifiedName other) 
        {
            return
                (Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase)) &&
                (NamespaceEquals(other.Namespace));
        }

        public bool NamespaceEquals(string? otherNamespace)
        {
            return
                ((Namespace == null) && (otherNamespace == null)) ||
                ((Namespace != null) && (otherNamespace != null) && (Namespace.Equals(otherNamespace, StringComparison.OrdinalIgnoreCase)));
        }

        public bool References(FullyQualifiedName referenced, HashSet<string>? usings)
        {
            if (!Name.Equals(referenced.Name, StringComparison.OrdinalIgnoreCase))
                return false;
            return ReferencesNamespace(referenced.Namespace, usings);
        }

        public bool ReferencesNamespace(string? referencedNamespace, HashSet<string>? usings)
        {
            var hasNamespace = !String.IsNullOrWhiteSpace(Namespace);
            var hasUsings = (usings != null) && (usings.Count > 0);
            if ((!hasNamespace) && (!hasUsings))
                return true;

            if (String.IsNullOrWhiteSpace(referencedNamespace))
                return !hasNamespace;

            return
                ((hasNamespace) && (Namespace!.Equals(referencedNamespace, StringComparison.OrdinalIgnoreCase))) ||
                ((!hasNamespace) && (hasUsings) && (usings!.Contains(referencedNamespace!)));
        }

    }

}
