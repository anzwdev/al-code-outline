using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public abstract class ObjectWithCodeSymbol : ObjectSymbol
    {

        public required List<GlobalVariableDeclarationSymbol> Variables { get; init; }
        public required List<MethodSymbol> Methods { get; init; }

        public ObjectWithCodeSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

        public ObjectWithCodeSymbol(ObjectIdentifier identifier, PropertySymbolsCollection properties)
            : base(identifier, properties)
        {
        }

    }
}
