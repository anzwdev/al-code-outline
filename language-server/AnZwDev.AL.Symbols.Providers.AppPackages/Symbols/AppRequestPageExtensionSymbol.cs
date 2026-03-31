using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal class AppRequestPageExtensionSymbol : AppSerializedSymbol<RequestPageExtensionSymbol>
    {

        public override RequestPageExtensionSymbol CreateSymbol(string? ns)
        {
            return new RequestPageExtensionSymbol()
            {
                ControlChanges = null,
                ActionChanges = null,
                Methods = null,
                Properties = null,
                Variables = null                
            };
        }

    }
}
