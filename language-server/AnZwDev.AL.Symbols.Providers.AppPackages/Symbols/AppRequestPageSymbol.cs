using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppRequestPageSymbol : AppSerializedSymbol<RequestPageSymbol>
    {

        public override RequestPageSymbol CreateSymbol(string? ns)
        {
            return new RequestPageSymbol()
            {
                Controls = null,
                Actions = null,
                Methods = null,
                Properties = null,
                Variables = null
            };
        }


    }
}
