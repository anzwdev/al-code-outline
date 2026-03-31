namespace AnZwDev.AL.Symbols
{
    public struct ObjectIdentifier
    {

        public ObjectKind ObjectKind { get; }
        public int Id { get; }
        public FullyQualifiedName FullyQualifiedName { get; }

        public string UniqueIdentifierKey { get; }
       
        public ObjectIdentifier(ObjectKind objectKind, int id, FullyQualifiedName fullyQualifiedName)
        {
            ObjectKind = objectKind;
            Id = id;
            FullyQualifiedName = fullyQualifiedName;
            UniqueIdentifierKey = CreateUniqueIdentifierKey();
        }

        public ObjectIdentifier(ObjectKind objectType, ObjectIdentifier source)
        {
            ObjectKind = objectType;
            Id = source.Id;
            FullyQualifiedName = source.FullyQualifiedName;
            UniqueIdentifierKey = CreateUniqueIdentifierKey();
        }

        public bool Equals(ObjectIdentifier other)
        {
            return
                (other.ObjectKind == ObjectKind) &&
                (FullyQualifiedName.Equals(other.FullyQualifiedName));
        }

        public bool IsEmpty()
        {
            return (Id == 0) && (FullyQualifiedName.IsEmpty());
        }

        public ObjectReference CreateReference()
        {
            return new ObjectReference(ObjectKind, null, Id, FullyQualifiedName, null);
        }

        public ObjectIdentifier CreateTableDataIdentifier()
        {
            if (ObjectKind != ObjectKind.Table)
                throw new InvalidOperationException("Only Table object can be converted to TableData identifier.");
            return new ObjectIdentifier(ObjectKind.TableData, this);
        }

        private string CreateUniqueIdentifierKey()
        {
            var uid = ObjectKind.ToString() + "|" + Id.ToString() + "|";
            if (!String.IsNullOrEmpty(FullyQualifiedName.Namespace))
                uid += FullyQualifiedName.Namespace.ToLower() + ".";
            uid += "\"" + FullyQualifiedName.Name.Replace("\"", "\"\"").ToLower() + "\"";
            return uid;
        }

    }
}
