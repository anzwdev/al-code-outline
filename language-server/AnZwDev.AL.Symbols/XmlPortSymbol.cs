using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class XmlPortSymbol : ObjectWithCodeSymbol
    {

        public required RequestPageSymbol? RequestPage { get; init; }
        public required List<XmlPortNodeSymbol>? Schema { get; init; }

        public XmlPortSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.XmlPort;
        }

    }

}
