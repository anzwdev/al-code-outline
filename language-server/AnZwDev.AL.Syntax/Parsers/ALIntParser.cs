using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Syntax.Parsers
{
    internal class ALIntParser
    {

        public int Parse(string? code, int defaultValue = 0)
        {
            if (String.IsNullOrEmpty(code))
                return defaultValue;

            if (Int32.TryParse(code, out int result))
                return result;

            return defaultValue;
        }

    }
}
