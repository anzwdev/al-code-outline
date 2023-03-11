using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.DuplicateCodeSearch
{
    public class DCDuplicateCollection : Dictionary<string, DCDuplicate>
    {

        public DCDuplicateCollection()
        {
        }

        public void Add(DCDuplicatePair pair)
        {
            string key = pair.DestinationCodeBlock.GetUniqueKey();
            DCDuplicate duplicate;
            if (this.ContainsKey(key))
                duplicate = this[key];
            else
            {
                duplicate = new DCDuplicate(pair.NoOfStatements, pair.CodeBlockType);
                duplicate.ranges.Add(pair.DestinationCodeBlock);
                this.Add(key, duplicate);
            }
            duplicate.ranges.Add(pair.SourceCodeBlock);
        }

    }
}
