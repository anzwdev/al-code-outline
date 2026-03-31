using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols
{
    public abstract class PropertySymbol
    {

        public required PropertyKind Kind { get; init; }

        public abstract string? GetStringValue();

    }

    public sealed class PropertyValueSymbol<T> : PropertySymbol
    {

        public required T Value { get; init; }


        public override string? GetStringValue()
        {
            return Value?.ToString();
        }

    }

}
