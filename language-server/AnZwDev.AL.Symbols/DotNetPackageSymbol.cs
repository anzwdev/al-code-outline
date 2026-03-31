using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class DotNetPackageSymbol : ObjectSymbol
    {

        public required List<DotNetAssemblyDeclarationSymbol>? AssemblyDeclarations { get; init; }

        public DotNetPackageSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties) : base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.DotNetPackage;
        }

    }

}
