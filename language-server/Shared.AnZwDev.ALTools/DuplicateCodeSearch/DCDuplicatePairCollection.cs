using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.DuplicateCodeSearch
{
    public class DCDuplicatePairCollection : Dictionary<string, DCDuplicatePair>
    {

        public DCDuplicatePairCollection()
        {
        }

        public void Add(DocumentRange sourceRange, DocumentRange destRange, int noOfStatements, DCCodeBlockType codeBlockType)
        {
            string sourceKey = sourceRange.GetUniqueKey();
            if (this.ContainsKey(sourceKey))
                this[sourceKey].DestinationCodeBlock = destRange;
            else
                this.Add(sourceKey, new DCDuplicatePair(sourceRange, destRange, noOfStatements, codeBlockType));
        }

    }
}
