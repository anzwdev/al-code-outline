using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts
{
    public class DocumentContentChangeRequest
    {

        public string? path { get; set; }
        public string? content { get; set; }
        public bool returnSymbols { get; set; }

    }
}
