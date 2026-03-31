using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.SystemSymbols
{
    public class SystemTypeSymbol
    {

        public NavTypeKind NavTypeKind { get; }
        public Dictionary<string, SystemMethodSymbol> Methods { get; } = new Dictionary<string, SystemMethodSymbol>();

        public SystemTypeSymbol(NavTypeKind navTypeKind, params SystemMethodSymbol[] methods)
        {
            NavTypeKind = navTypeKind;
            for (int i = 0; i < methods.Length; i++)
                Methods.Add(methods[i].Name.ToLower(), methods[i]);
        }

        public SystemMethodSymbol? GetMethod(string? name)
        {
            if (name != null)
            {
                name = name.ToLower();
                if (Methods.ContainsKey(name))
                    return Methods[name];
            }
            return null;
        }


    }
}
