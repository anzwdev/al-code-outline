using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class GetProjectSymbolLocationRequest
    {

        public string projectPath { get; set; }
        public string kind { get; set; }
        public string name { get; set; }

    }
}
