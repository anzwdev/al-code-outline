using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.DuplicateCodeSearch
{
    public class DCStatementKey
    {
        public bool Ignore { get; }
        public string Value { get; }
        public List<DCStatementInstance> StatementInstances { get; } = new List<DCStatementInstance>();

        public DCStatementKey(string value, bool ignore)
        {
            this.Value = value;
            this.Ignore = ignore;
        }

    }
}
