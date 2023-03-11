using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class DocumentSymbolsRequest
    {

        public string source { get; set; }
        public string path { get; set; }
        public bool includeProperties { get; set; }
        public bool isActiveDocument { get; set; }

    }
}
