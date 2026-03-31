using System.Xml.Linq;

namespace AnZwDev.AL.Symbols
{
    public struct ObjectReference
    {

        public string? AppId { get; }
        public ObjectKind ObjectKind { get; }
        public HashSet<string>? Usings { get; }
        public int ObjectId { get; }
        public FullyQualifiedName FullyQualifiedName { get; }

        public bool HasUsings
        {
            get { return ((Usings != null) && (Usings.Count > 0)); }
        }

        public bool IsEmpty()
        {
            return (ObjectKind == ObjectKind.Unknown) && (ObjectId == 0) && (FullyQualifiedName.IsEmpty());
        }

        public ObjectReference(ObjectKind objectKind, string? appId, int objectId, FullyQualifiedName fullyQualifiedName, HashSet<string>? usings)
        {
            AppId = appId;
            ObjectKind = objectKind;
            Usings = usings;
            ObjectId = objectId;
            FullyQualifiedName = fullyQualifiedName;
        }

        public ObjectReference(ObjectKind objectKind, string? appId, int objectId, HashSet<string>? usings) : 
            this(objectKind, appId, objectId, new FullyQualifiedName() { Name = String.Empty, Namespace = null }, usings)
        {
        }

        public ObjectReference(ObjectKind objectKind, string? appId, FullyQualifiedName fullyQualifiedName, HashSet<string>? usings) : 
            this(objectKind, appId, 0, fullyQualifiedName, usings)
        {
        }

        public bool References(ObjectIdentifier objectIdentifier)
        {
            if (this.ObjectKind != objectIdentifier.ObjectKind)
                return false;

            return (
                ((ObjectId != 0) && (ObjectId == objectIdentifier.Id)) ||
                (FullyQualifiedName.References(objectIdentifier.FullyQualifiedName, Usings))
                );
        }

        public bool ReferencesNamespace(string? namespaceName)
        {
            return FullyQualifiedName.ReferencesNamespace(namespaceName, Usings);
        }

    }
}
