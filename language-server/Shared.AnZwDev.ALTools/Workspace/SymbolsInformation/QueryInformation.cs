using AnZwDev.ALTools.ALSymbolReferences;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Workspace.SymbolsInformation
{
    public class QueryInformation : SymbolWithIdInformation
    {

        public QueryInformation()
        {
        }

        public QueryInformation(ALAppQuery query) : base(query)
        {
            if (query.Properties != null)
                this.Caption = query.Properties.GetValue("Caption");
        }

    }
}
