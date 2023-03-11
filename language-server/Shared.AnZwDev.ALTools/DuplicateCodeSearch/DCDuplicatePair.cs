using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.DuplicateCodeSearch
{
    public class DCDuplicatePair
    {

        public DocumentRange SourceCodeBlock { get; }
        public DocumentRange DestinationCodeBlock { get; set; }
        public int NoOfStatements { get; }
        public DCCodeBlockType CodeBlockType { get; set; }

        public DCDuplicatePair(DocumentRange source, DocumentRange dest, int noOfStatements, DCCodeBlockType codeBlockType)
        {
            this.SourceCodeBlock = source;
            this.DestinationCodeBlock = dest;   
            this.NoOfStatements = noOfStatements;
            this.CodeBlockType = codeBlockType;
        }


    }
}
