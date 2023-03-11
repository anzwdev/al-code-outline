using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server.Contracts.ChangeTracking
{
    public class DocumentContentChangeRequest
    {

        public string path { get; set; }
        public string content { get; set; }
        public bool returnSymbols { get; set; }

    }
}
