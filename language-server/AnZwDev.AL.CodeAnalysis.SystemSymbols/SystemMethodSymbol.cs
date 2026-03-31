using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.SystemSymbols
{
    public class SystemMethodSymbol
    {

        public string Name { get; }
        public SystemMethodParameterSymbol[] Parameters { get; }

        public SystemMethodSymbol(string name, params SystemMethodParameterSymbol[] parameters)
        {
            Name = name;
            Parameters = parameters;
        }

    }
}
