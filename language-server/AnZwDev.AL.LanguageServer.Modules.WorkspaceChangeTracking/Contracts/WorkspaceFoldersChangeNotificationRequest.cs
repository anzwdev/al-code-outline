using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts
{
    public class WorkspaceFoldersChangeNotificationRequest
    {

        public ProjectSource[]? added { get; set; }
        public string[]? removed { get; set; }

    }
}
