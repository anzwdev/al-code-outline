using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking.Contracts
{
    public class ConfigurationChangeNotificationRequest
    {
        public ProjectSource[]? updatedProjects { get; set; }
    }
}
