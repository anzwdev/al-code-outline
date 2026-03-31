using AnZwDev.AL.Syntax.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Syntax
{
    public static class ALLiteralParser
    {

        private static readonly ALBoolParser _alBoolParser = new ALBoolParser();
        private static readonly ALStringParser _alStringParser = new ALStringParser();
        private static readonly ALNameParser _alNameParser = new ALNameParser();
        private static readonly ALIntParser _alIntParser = new ALIntParser();

        public static int ParseInt(string? code)
        {
            return _alIntParser.Parse(code);
        }

        public static bool ParseBool(string? code, bool defaultValue = false)
        {
            return _alBoolParser.Parse(code, defaultValue);
        }

        public static string ParseString(string? code)
        {
            return _alStringParser.Parse(code);
        }

        public static string ParseName(string? code)
        {
            return _alNameParser.Parse(code);
        }

        public static bool IsValidName(string? code)
        {
            return _alNameParser.IsValid(code);
        }

    }
}
