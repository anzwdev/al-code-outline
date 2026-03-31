using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using AnZwDev.AL.Syntax;

namespace AnZwDev.AL.Symbols.Parsing.Parsers
{
    internal class ALEnumParser<T> where T : struct
    {

        public virtual T Default { get; }

        public ALEnumParser(T defaultValue)
        {
            this.Default = defaultValue;
        }

        public virtual bool TryParse(string? value, out T result)
        {
            if (!String.IsNullOrWhiteSpace(value))
                return Enum.TryParse(ALLiteralParser.ParseName(value), true, out result);

            result = default;
            return false;
        }

        public T Parse(string? value)
        {
            if (TryParse(value, out var result))
                return result;
            return Default;
        }

    }
}
