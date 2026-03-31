using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public abstract class ObjectExtensionWithCodeSymbol : ObjectExtensionSymbol
    {

        public required List<GlobalVariableDeclarationSymbol> Variables { get; init; }
        public required List<MethodSymbol> Methods { get; init; }

        public ObjectExtensionWithCodeSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

    }
}
