using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Symbols.Parsing.Parsers
{
    internal class FuncPropertyValueParser<T> : PropertyValueParser<T>
    {

        private readonly Func<string?, Dictionary<string, string>?, T> _parseFunction;

        public FuncPropertyValueParser(PropertyKind kind, Func<string?, Dictionary<string, string>?, T> parseFunction) :
            base(kind)
        {
            _parseFunction = parseFunction;
        }

        protected override T ParseValue(string? value, Dictionary<string, string>? valueProperties)
        {
            return _parseFunction(value, valueProperties);
        }

    }
}
