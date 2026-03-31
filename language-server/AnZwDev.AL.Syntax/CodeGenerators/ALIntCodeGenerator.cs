using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Syntax.Formatters
{
    internal class ALIntFormatter : ALSyntaxElementStringFormatter<int>
    {

        public override string Get(int element)
        {
            return element.ToString();
        }

    }
}
