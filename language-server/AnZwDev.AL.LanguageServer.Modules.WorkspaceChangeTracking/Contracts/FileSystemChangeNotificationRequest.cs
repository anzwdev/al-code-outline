using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts
{
    public class FileSystemChangeNotificationRequest
    {
        public string? path { get; set; }
    }
}
