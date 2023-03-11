using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{
    public static class CharExtensions
    {

        public static bool IsNewLine(this char character)
        {
            return (character == '\n') || (character == '\r');
        }

        public static bool IsPatternEqualIgnoreCase(this char character, char patternCharacter)
        {
            return (patternCharacter == '?') || (Char.ToUpper(character) == Char.ToUpper(patternCharacter));
        }

        public static bool IsPatternEqual(this char character, char patternCharacter)
        {
            return (patternCharacter == '?') || (character == patternCharacter);
        }

    }
}
