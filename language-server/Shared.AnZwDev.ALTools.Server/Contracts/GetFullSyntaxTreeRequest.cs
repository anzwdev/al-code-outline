using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class GetFullSyntaxTreeRequest
    {

        public string source { get; set; }
        public string path { get; set; }


    }
}
