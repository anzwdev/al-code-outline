using AnZwDev.AL.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis.SystemSymbols
{
    public class SystemMethodParameterSymbol
    {

        public int Index { get; }
        public ObjectKind ObjectType { get; }

        public SystemMethodParameterSymbol(int index, ObjectKind objectType)
        {
            Index = index;
            ObjectType = objectType;
        }


    }
}
