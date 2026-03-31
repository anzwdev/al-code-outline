using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public abstract class ObjectSymbol : Symbol
    {

        public ObjectIdentifier Identifier { get; }
        public AccessLevel AccessLevel { get; }
        public PropertySymbolsCollection Properties { get; }
        public required string? ReferenceSourceFileName { get; set; }
        public required HashSet<string>? Usings { get; init; }

        public ObjectSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
        {
            Identifier = new ObjectIdentifier(GetObjectType(), id, fullyQualifiedName);
            Properties = properties;
            AccessLevel = properties.Access;
        }

        protected ObjectSymbol(ObjectIdentifier identifier, PropertySymbolsCollection properties)
        {
            Identifier = identifier;
            Properties = properties;
            AccessLevel = properties.Access;
        }

        protected abstract ObjectKind GetObjectType();

        public bool HasFullInherentPermissions()
        {
            var permissions = this.Properties.InherentPermissions;
            return (permissions != null) && (permissions.IndexOf("X") >= 0);
        }

    }
}
