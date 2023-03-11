using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.DuplicateCodeSearch
{
    public class DCStatementInstance
    {
        public DCStatementsBlock StatementsBlock { get; }
        public DCStatementKey Key { get; }
        public Range Range { get; }
        public int StatementInstanceIndex { get; }

        public DCStatementInstance(DCStatementsBlock statementsBlock, DCStatementKey key, Range range, int statementInstanceIndex)
        {
            this.StatementsBlock = statementsBlock;
            this.Key = key;
            this.Range = range;
            this.StatementInstanceIndex = statementInstanceIndex;
        }

    }
}
