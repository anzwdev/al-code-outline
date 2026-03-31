using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Syntax.Formatters
{
    public abstract class ALSyntaxElementFormatter<T>
    {

        public abstract string Get(T element);
        public abstract void Write(TextWriter writer, T element);

    }
}
