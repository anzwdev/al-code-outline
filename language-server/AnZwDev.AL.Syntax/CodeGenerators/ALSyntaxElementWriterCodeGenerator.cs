using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Syntax.Formatters
{
    public abstract class ALSyntaxElementWriterFormatter<T> : ALSyntaxElementFormatter<T>
    {

        public override string Get(T element)
        {
            using (var writer = new StringWriter())
            {
                Write(writer, element);
                return writer.ToString();
            }
        }

    }
}
