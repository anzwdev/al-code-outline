using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class GetSyntaxTreeSymbolRequest
    {

        public string path { get; set; }
        public int[] symbolPath { get; set; }

    }
}
