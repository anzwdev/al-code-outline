using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.DuplicateCodeSearch
{
    public class DCStatementsBlock
    {
        public string SourceFilePath { get; }
        public DCCodeBlockType CodeBlockType { get; }

        public List<DCStatementInstance> Statements { get; } = new List<DCStatementInstance>();

        public DCStatementsBlock(string sourceFilePath, DCCodeBlockType codeBlockType)
        {
            this.SourceFilePath = sourceFilePath;
            this.CodeBlockType = codeBlockType;
        }

    }
}
