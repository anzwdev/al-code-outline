using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AnZwDev.AL.Syntax.Formatters
{
    internal class ALKeywordFormatter : ALSyntaxElementStringFormatter<string>
    {

        public override string Get(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return name;

            string nameText = name.ToLower();
            switch (nameText)
            {
                case "addfirst": return "AddFirst";
                case "addlast": return "AddLast";
                case "addbefore": return "AddBefore";
                case "addafter": return "AddAfter";
                case "dataset": return "DataSet";
            }
            return name;
        }

    }
}
