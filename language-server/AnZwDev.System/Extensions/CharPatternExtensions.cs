using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.Extensions
{
    public static class CharPatternExtensions
    {

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
