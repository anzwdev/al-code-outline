using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Syntax.Parsers
{
    internal class ALBoolParser
    {

        public bool Parse(string? code, bool defaultValue)
        {
            if (code != null)
            {
                code = code.ToLower();
                if ((code == "1") || (code == ALLanguageFacts.BooleanTrueLiteral) || (code == "yes"))
                    return true;
                if ((code == "0") || (code == ALLanguageFacts.BooleanFalseLiteral) || (code == "no"))
                    return false;
            }
            return defaultValue;
        }

    }
}
