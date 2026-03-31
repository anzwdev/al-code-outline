using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Syntax.Formatters
{
    public abstract class ALSyntaxElementStringFormatter<T> : ALSyntaxElementFormatter<T>
    {

        public override void Write(TextWriter writer, T element)
        {
            writer.Write(Get(element));
        }  

    }
}
