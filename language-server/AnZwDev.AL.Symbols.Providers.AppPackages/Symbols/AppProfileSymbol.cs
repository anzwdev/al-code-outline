using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppProfileSymbol : AppObjectWithIdSymbol<ProfileSymbol>
    {

        public override ProfileSymbol CreateSymbol(string? ns)
        {
            return new ProfileSymbol(Id, new FullyQualifiedName(ns, Name ?? String.Empty), AppPropertySymbol.CreatePropertySymbolsCollection(Properties))
            { 
                ReferenceSourceFileName = ReferenceSourceFileName,
                Usings = null
            };
        }

    }
}
