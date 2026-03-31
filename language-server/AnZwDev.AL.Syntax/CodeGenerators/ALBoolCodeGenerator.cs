using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Syntax.Formatters
{
    internal class ALBoolFormatter : ALSyntaxElementStringFormatter<bool>
    {

        public override string Get(bool element)
        {
            return element ? ALLanguageFacts.BooleanTrueLiteral : ALLanguageFacts.BooleanFalseLiteral;
        }

    }
}
