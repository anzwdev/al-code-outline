using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.DuplicateCodeSearch
{
    public class DCDuplicate
    {
        public int noOfStatements { get; }
        public DCCodeBlockType codeBlockType { get; }
        public List<DocumentRange> ranges { get; } = new List<DocumentRange>();

        public DCDuplicate(int newNoOfStatements, DCCodeBlockType newCodeBlockType)
        {
            this.noOfStatements = newNoOfStatements;
            this.codeBlockType = newCodeBlockType;
        }

    }
}
