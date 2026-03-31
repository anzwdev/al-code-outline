using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Syntax.Formatters
{
    internal class ALNameFormatter
    {

        public void Write(TextWriter writer, string name, bool force)
        {
            writer.Write(Get(name, force));
        }

        public string Get(string name, bool force)
        {
            if ((force) || (NameNeedsEncoding(name)))
                return
                    ALLanguageFacts.NameDelimiterString +
                    name.Replace(ALLanguageFacts.NameDelimiterString, ALLanguageFacts.NameDelimiterEscapeString) +
                    ALLanguageFacts.NameDelimiterString;
            return name;
        }

        public string GetList(List<string> names, string separator, bool force)
        {
            if (names.Count == 0)
                return "";

            if (names.Count == 1)
                return Get(names[0], force);

            using (var writer = new StringWriter())
            {
                WriteList(writer, names, separator, force);
                return writer.ToString();
            }
        }

        public void WriteList(TextWriter writer, List<string> names, string separator, bool force)
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (i > 0)
                    writer.Write(separator);
                Write(writer, names[i], force);
            }
        }

        private static bool NameNeedsEncoding(string name)
        {
            if (name.Length > 0)
            {
                if (!ALLanguageFacts.IsValidNameFirstCharacter(name[0]))
                    return true;
                for (int i = 1; i < name.Length; i++)
                    if (!ALLanguageFacts.IsValidNameMiddleCharacter(name[i]))
                        return true;
            }
            return false;
        }

    }
}
