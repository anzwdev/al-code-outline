using AnZwDev.ALTools.ALSymbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts
{
    public class DocumentPositionRequest
    {

        public bool isActiveDocument { get; set; }
        public string source { get; set; }
        public Position position { get; set; }

    }
}
