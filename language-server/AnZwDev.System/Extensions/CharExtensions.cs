using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.System.Extensions
{
    public static class CharExtensions
    {

        public static bool IsNewLine(this char character)
        {
            return (character == '\n') || (character == '\r');
        }

        public static bool IsValidWordCharacter(this char c)
        {
            return
                ((c >= 'A') && (c <= 'Z')) ||
                ((c >= 'a') && (c <= 'z')) ||
                ((c >= '0') && (c <= '9')) ||
                (c == '_');
        }

    }
}
