using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace AnZwDev.AL.Syntax
{
    public class ALNameNormalizer
    {

        public static string Normalize(string name)
        {
            if (name.Length == 0)
                return name;

            var builder = new StringBuilder();
            Normalize(name, builder);
            return builder.ToString();
        }

        public static void Normalize(string name, StringBuilder builder)
        {
            if (name.Length == 0)
                return;

            char lastChar = name[0];
            if (!ALLanguageFacts.IsValidNameFirstCharacter(name[0]))
                lastChar = ALLanguageFacts.InvalidCharacterReplacementChar;
            builder.Append(lastChar);

            for (int i = 1; i < name.Length; i++)
            {
                var currentChar = name[i];
                if (!ALLanguageFacts.IsValidNameMiddleCharacter(currentChar))
                    currentChar = ALLanguageFacts.InvalidCharacterReplacementChar;
                if ((currentChar != ALLanguageFacts.InvalidCharacterReplacementChar) || (currentChar != lastChar))
                    builder.Append(currentChar);
                lastChar = currentChar;
            }
        }


    }
}
