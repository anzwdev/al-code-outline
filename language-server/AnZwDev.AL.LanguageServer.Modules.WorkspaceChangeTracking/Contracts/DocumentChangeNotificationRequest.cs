using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts
{
    public class DocumentChangeNotificationRequest
    {
        public string? path { get; set; }
        public string? content { get; set; }
    }
}
