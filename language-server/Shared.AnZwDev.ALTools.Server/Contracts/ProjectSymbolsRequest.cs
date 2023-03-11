using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class ProjectSymbolsRequest
    {

        public bool includeDependencies { get; set; }
        public string projectPath { get; set; }
        public string packagesFolder { get; set; }
        public string[] workspaceFolders { get; set; }

    }
}
