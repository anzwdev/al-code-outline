using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Syntax
{
    public class ALNamespaceNormalizer
    {

        public static string Normalize(string name)
        {
            //skip spaces or namespace parts separators at the beginning of the name
            var parts = name.Split(ALLanguageFacts.FullyQualifiedNameSeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            var builder = new StringBuilder();

            for (int i = 0; i < parts.Length; i++)
            {
                ALNameNormalizer.Normalize(parts[i], builder);
                if (i < (parts.Length - 1))
                    builder.Append(ALLanguageFacts.FullyQualifiedNameSeparatorChar);
            }

            return builder.ToString();
        }

    }
}
