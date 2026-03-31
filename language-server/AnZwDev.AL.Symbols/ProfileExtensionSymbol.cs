using AnZwDev.AL.Symbols.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{

    public class ProfileExtensionSymbol : ObjectExtensionSymbol
    {

        public ProfileExtensionSymbol(int id, FullyQualifiedName fullyQualifiedName, PropertySymbolsCollection properties)
            : base(id, fullyQualifiedName, properties)
        {
        }

        protected override ObjectKind GetObjectType()
        {
            return ObjectKind.ProfileExtension;
        }

        protected override ObjectKind GetExtendedObjectType()
        {
            return ObjectKind.Profile;
        }

    }

}
