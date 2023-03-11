using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class WorkspaceCommandResponse
    {

        public string source { get; set; }
        public bool error { get; set; }
        public string errorMessage { get; set; }
        public Dictionary<string, string> parameters { get; set; }

    }
}
